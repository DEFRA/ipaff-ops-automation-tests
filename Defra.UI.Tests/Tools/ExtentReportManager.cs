using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Defra.UI.Tests.Tools
{
    /// <summary>
    /// Builds a single ExtentReports HTML report per pipeline job.
    ///
    /// To support the retry pipeline (which spawns a fresh <c>dotnet test</c> process for each
    /// retry attempt) without ending up with multiple disjoint HTML reports, this manager:
    ///   1. Writes a JSON archive (<c>TestExecutionArchive.json</c>) of every run.
    ///   2. On startup, loads any pre-existing archive so the previous attempt's results carry over.
    ///   3. Emits the HTML to a STABLE filename (<c>TestExecutionReport.html</c>) — each retry
    ///      run overwrites it with the accumulated view, so the final file contains every attempt
    ///      complete with steps, screenshots and scenario-context logs.
    ///   4. At end of run, <see cref="FinalizeReport"/> collapses duplicate scenarios produced by
    ///      earlier retry attempts so the final HTML shows only the latest outcome per test,
    ///      annotated with a retry-history note. The cleaned archive is written back so the next
    ///      retry process loads a single baseline entry per scenario.
    ///
    /// When <c>enableRetry</c> is OFF in the pipeline, the archive is created once and the single
    /// HTML report is the deliverable — no separate "_Merged" file is produced.
    /// </summary>
    public class ExtentReportManager
    {
        private const string HtmlReportName = "TestExecutionReport.html";
        private const string JsonArchiveName = "TestExecutionArchive.json";

        private static ExtentReports _extent;
        private static ExtentSparkReporter _htmlReporter;
        private static ExtentJsonFormatter _jsonReporter;

        public static ExtentReports GetInstance()
        {
            if (_extent == null)
            {
                var reportDir = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "Reports");

                if (!Directory.Exists(reportDir))
                {
                    Directory.CreateDirectory(reportDir);
                }

                var htmlPath = Path.Combine(reportDir, HtmlReportName);
                var archivePath = Path.Combine(reportDir, JsonArchiveName);

                _extent = new ExtentReports();

                // Load a previous run's results so the current attempt is appended to the same
                // cumulative report. This is what enables retry attempts to roll up into one HTML.
                if (File.Exists(archivePath))
                {
                    try
                    {
                        _extent.CreateDomainFromJsonArchive(archivePath);
                    }
                    catch
                    {
                        // Archive corrupt or incompatible — start fresh rather than fail the run.
                    }
                }

                // Persist this run's results for any subsequent retry attempts to pick up.
                try
                {
                    _jsonReporter = new ExtentJsonFormatter(archivePath);
                    _extent.AttachReporter(_jsonReporter);
                }
                catch
                {
                    // JsonFormatter unavailable in this ExtentReports build — HTML still generates,
                    // but retry runs will not be able to merge into a single report.
                }

                // Stable HTML filename so retry runs overwrite the same file with the cumulative view.
                _htmlReporter = new ExtentSparkReporter(htmlPath);
                _extent.AttachReporter(_htmlReporter);
            }

            return _extent;
        }

        /// <summary>
        /// Collapses duplicate scenarios produced by earlier retry attempts. Keeps only the
        /// latest result per (Feature, Scenario) pair, annotates the survivor with a retry-history
        /// note, writes the cleaned data back to the JSON archive and re-renders the HTML.
        ///
        /// Call once at end of run from a Reqnroll <c>[AfterTestRun]</c> hook. Safe to call when
        /// no duplicates exist — in that case the archive and HTML are left untouched.
        /// </summary>
        public static void FinalizeReport()
        {
            if (_extent == null)
            {
                return;
            }

            var reportDir = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "Reports");
            var archivePath = Path.Combine(reportDir, JsonArchiveName);
            var htmlPath = Path.Combine(reportDir, HtmlReportName);

            if (!File.Exists(archivePath))
            {
                return;
            }

            try
            {
                if (!TryDeduplicateArchive(archivePath, out var dedupedJson))
                {
                    // Nothing to dedupe (single run / no duplicates) — leave existing outputs alone.
                    return;
                }

                File.WriteAllText(archivePath, dedupedJson);

                // Re-render the HTML from the cleaned archive. Use a fresh ExtentReports instance
                // with ONLY an HTML reporter — re-attaching the JSON formatter would replay the
                // original (duplicated) in-memory tree from this process back into the archive.
                var freshExtent = new ExtentReports();
                freshExtent.CreateDomainFromJsonArchive(archivePath);

                var freshHtml = new ExtentSparkReporter(htmlPath);
                freshExtent.AttachReporter(freshHtml);
                freshExtent.Flush();
            }
            catch
            {
                // Report finalisation must never break the build — worst case duplicates remain
                // in the HTML, but the underlying test pass/fail signal is unaffected.
            }
        }

        /// <summary>
        /// Reads <paramref name="archivePath"/>, drops scenarios superseded by a later retry
        /// attempt with the same Feature+Scenario name, and annotates the surviving scenario's
        /// Description with a retry-history note. Returns <c>true</c> with the cleaned JSON when
        /// duplicates were found; returns <c>false</c> otherwise.
        /// </summary>
        private static bool TryDeduplicateArchive(string archivePath, out string dedupedJson)
        {
            dedupedJson = null;

            var raw = File.ReadAllText(archivePath);
            if (string.IsNullOrWhiteSpace(raw))
            {
                return false;
            }

            JsonNode root;
            try
            {
                root = JsonNode.Parse(raw);
            }
            catch
            {
                return false;
            }

            if (root is not JsonArray features || features.Count == 0)
            {
                return false;
            }

            // key = "Feature||Scenario", value = winning scenario + history of prior attempts.
            var winners = new Dictionary<string, ScenarioWinner>(StringComparer.Ordinal);

            // Preserve one Feature node template per Feature name so we can rebuild the tree with
            // consistent Feature-level metadata (BehaviorDrivenType, Tags, StartTime, etc).
            var featureTemplates = new Dictionary<string, JsonObject>(StringComparer.Ordinal);

            foreach (var node in features)
            {
                if (node is not JsonObject feature)
                {
                    continue;
                }

                var featureName = GetStringProp(feature, "Name");
                if (string.IsNullOrEmpty(featureName))
                {
                    continue;
                }

                featureTemplates[featureName] = feature;

                if (GetProp(feature, "Children") is not JsonArray scenarios)
                {
                    continue;
                }

                foreach (var sc in scenarios)
                {
                    if (sc is not JsonObject scenario)
                    {
                        continue;
                    }

                    var scenarioName = GetStringProp(scenario, "Name");
                    if (string.IsNullOrEmpty(scenarioName))
                    {
                        continue;
                    }

                    var status = GetStringProp(scenario, "Status") ?? string.Empty;
                    var endTime = ParseDate(GetStringProp(scenario, "EndTime"));
                    var key = $"{featureName}||{scenarioName}";

                    if (winners.TryGetValue(key, out var existing))
                    {
                        if (endTime > existing.EndTime)
                        {
                            // New attempt is later — demote the previous survivor into history.
                            existing.PriorStatuses.Add(GetStringProp(existing.Scenario, "Status") ?? string.Empty);
                            winners[key] = new ScenarioWinner
                            {
                                Scenario = scenario,
                                EndTime = endTime,
                                FeatureName = featureName,
                                PriorStatuses = existing.PriorStatuses,
                            };
                        }
                        else
                        {
                            // Older attempt — record it as history against the current survivor.
                            existing.PriorStatuses.Add(status);
                        }
                    }
                    else
                    {
                        winners[key] = new ScenarioWinner
                        {
                            Scenario = scenario,
                            EndTime = endTime,
                            FeatureName = featureName,
                            PriorStatuses = new List<string>(),
                        };
                    }
                }
            }

            var anyDuplicates = winners.Values.Any(w => w.PriorStatuses.Count > 0);
            if (!anyDuplicates)
            {
                return false;
            }

            // Annotate each survivor with a retry-history note (visible via the Description field,
            // which the Spark HTML template renders near the test title).
            foreach (var winner in winners.Values)
            {
                if (winner.PriorStatuses.Count == 0)
                {
                    continue;
                }

                var attemptNumber = winner.PriorStatuses.Count;
                var finalStatus = GetStringProp(winner.Scenario, "Status") ?? "Unknown";
                var note = $"<b>{finalStatus} on retry attempt {attemptNumber}</b> " +
                           $"(previous attempts: {string.Join(", ", winner.PriorStatuses)})";

                var existingDesc = GetStringProp(winner.Scenario, "Description");
                winner.Scenario["Description"] = string.IsNullOrEmpty(existingDesc)
                    ? note
                    : $"{note}<br/>{existingDesc}";
            }

            // Rebuild the features array — one Feature per name, scenarios = winners only.
            var deduped = new JsonArray();
            foreach (var group in winners.Values.GroupBy(w => w.FeatureName, StringComparer.Ordinal))
            {
                if (!featureTemplates.TryGetValue(group.Key, out var template))
                {
                    continue;
                }

                // JsonNode instances may have only one parent — deep-clone via re-parse so we can
                // re-attach freely without detaching from the original tree.
                var clonedFeature = (JsonObject)JsonNode.Parse(template.ToJsonString());

                var newChildren = new JsonArray();
                foreach (var winner in group)
                {
                    newChildren.Add(JsonNode.Parse(winner.Scenario.ToJsonString()));
                }
                clonedFeature["Children"] = newChildren;

                deduped.Add(clonedFeature);
            }

            dedupedJson = deduped.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
            return true;
        }

        private static string GetStringProp(JsonObject obj, string name)
        {
            var node = GetProp(obj, name);
            return node?.ToString();
        }

        private static JsonNode GetProp(JsonObject obj, string name)
        {
            if (obj.TryGetPropertyValue(name, out var node))
            {
                return node;
            }

            // Case-insensitive fallback in case the serialiser ever changes casing convention.
            foreach (var kvp in obj)
            {
                if (string.Equals(kvp.Key, name, StringComparison.OrdinalIgnoreCase))
                {
                    return kvp.Value;
                }
            }

            return null;
        }

        private static DateTime ParseDate(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return DateTime.MinValue;
            }

            return DateTime.TryParse(
                s,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.AssumeUniversal,
                out var dt)
                ? dt
                : DateTime.MinValue;
        }

        private sealed class ScenarioWinner
        {
            public JsonObject Scenario { get; set; }
            public DateTime EndTime { get; set; }
            public string FeatureName { get; set; }
            public List<string> PriorStatuses { get; set; } = new();
        }
    }
}