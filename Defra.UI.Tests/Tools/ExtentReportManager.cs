using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System.Reflection;

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
    }
}