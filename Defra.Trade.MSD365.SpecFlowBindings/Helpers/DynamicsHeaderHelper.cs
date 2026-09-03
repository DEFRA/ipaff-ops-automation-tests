using System;
using System.Threading;
using OpenQA.Selenium;
using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Defra.Trade.MSD365.SpecFlowBindings.Helpers
{
    /// <summary>
    /// Helper for reading values from Dynamics 365 UCI record header fields.
    /// </summary>
    /// <remarks>
    /// Modern Dynamics UCI renders header fields in one of several shapes, depending on record state
    /// and available horizontal space:
    /// <list type="number">
    /// <item>
    /// Inline Web Component: a &lt;uci-header-control-list-item&gt; custom element whose value anchor is
    /// projected via a &lt;slot&gt;. The anchor lives in the element's LIGHT DOM (as a child of the host),
    /// not inside the shadow root — the shadow root contains only &lt;slot&gt; placeholders — so it must
    /// be queried from the host element itself, not from host.shadowRoot.
    /// </item>
    /// <item>
    /// Collapsed into the "More Header Editable Fields" overflow flyout: when there isn't enough
    /// horizontal space, Dynamics moves fields into a flyout that only exists in the DOM once the
    /// header_overflowButton has been clicked. Inside the flyout, fields render as classic Dynamics
    /// field controls — lookup tag divs (LookupResultsDropdown_..._selected_tag_text) or Fluent UI
    /// readonly &lt;input&gt; controls for OptionSet fields (where the display value lives in the
    /// 'value' attribute rather than as rendered text).
    /// </item>
    /// </list>
    /// When a modal dialog (e.g. a Work Order Task popup) is open over an underlying record page,
    /// multiple header overflow buttons can exist in the DOM simultaneously. The active/topmost one
    /// is the last in document order, and clicks are issued via JavaScript to avoid
    /// ElementClickInterceptedException caused by the covered background button.
    /// </remarks>
    internal class DynamicsHeaderHelper
    {
        private const string OverflowButtonXPath = "//button[@data-id='header_overflowButton']";

        private readonly IWebDriver driver;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicsHeaderHelper"/> class.
        /// </summary>
        /// <param name="driver">Selenium IWebDriver</param>
        public DynamicsHeaderHelper(IWebDriver driver)
        {
            this.driver = driver;
        }

        /// <summary>
        /// Reads a header field's current display text where the field is rendered as an inline
        /// &lt;uci-header-control-list-item&gt; Web Component with a slotted value anchor (e.g.
        /// Substatus, Owner on a Work Order), falling back to the "More Header Editable Fields"
        /// overflow flyout — where the same field instead renders as a classic Dynamics lookup tag
        /// control — if it isn't rendered inline.
        /// </summary>
        /// <param name="headerDataName">The data-name attribute of the inline uci-header-control-list-item (e.g. "header_msdyn_substatus").</param>
        /// <param name="flyoutFieldSchemaName">The field schema name used in the flyout's data-id pattern (e.g. "msdyn_substatus", "ownerid").</param>
        /// <param name="slotName">The slot name used for the inline anchor (defaults to "value").</param>
        /// <param name="flyoutTimeoutSeconds">Max time to wait for the flyout's value element to render after expanding.</param>
        /// <returns>The field's display text, or null if it could not be found.</returns>
        public string GetHeaderFieldValueText(
            string headerDataName,
            string flyoutFieldSchemaName,
            string slotName = "value",
            int flyoutTimeoutSeconds = 15)
        {
            driver.SwitchTo().DefaultContent();

            var findInlineScript = $@"
                var host = document.querySelector(""uci-header-control-list-item[data-name='{headerDataName}']"");
                if (!host) return null;
                return host.querySelector(""a[slot='{slotName}']"") || host.querySelector(""a.value-link"");
            ";

            var inlineElement = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript(findInlineScript);

            if (inlineElement != null)
            {
                return inlineElement.Text.Trim();
            }

            // Not rendered inline — collapsed into the "More Header Editable Fields" overflow flyout.
            var expandButton = GetActiveHeaderOverflowButton();

            if (expandButton == null)
            {
                return null;
            }

            var wasAlreadyExpanded = expandButton.GetAttribute("aria-expanded") == "true";

            if (!wasAlreadyExpanded)
            {
                ClickViaJavaScript(expandButton);
                driver.WaitForTransaction();
            }

            string valueText = null;
            var deadline = DateTime.UtcNow.AddSeconds(flyoutTimeoutSeconds);

            while (DateTime.UtcNow < deadline && valueText == null)
            {
                var candidates = driver.FindElements(By.XPath(
                    $"//div[contains(@data-id,'LookupResultsDropdown_{flyoutFieldSchemaName}_selected_tag_text')]"));

                if (candidates.Count > 0)
                {
                    // Capture text now, while the element is still attached — it goes stale once
                    // the flyout is collapsed below.
                    valueText = candidates[^1].Text.Trim();
                    break;
                }

                Thread.Sleep(TimeSpan.FromMilliseconds(500));
            }

            CollapseHeaderFieldsFlyoutIfOpen();

            return valueText;
        }

        /// <summary>
        /// Reads a header field's current display value by matching its visible &lt;label&gt; text,
        /// covering fields rendered as either a readonly Fluent UI &lt;input&gt; (OptionSet controls,
        /// e.g. Status, Status Reason, HMI/PHSI Inspection Required, Inspection Classification) or a
        /// classic lookup tag control. Falls back to opening the "More Header Editable Fields"
        /// overflow flyout if the field isn't rendered inline.
        /// </summary>
        /// <param name="fieldLabel">The exact visible label text of the field (e.g. "Status", "HMI Inspection Required").</param>
        /// <param name="flyoutTimeoutSeconds">Max time to wait for the flyout's value element to render after expanding.</param>
        /// <returns>The field's display value, or null if it could not be found.</returns>
        public string GetHeaderFieldValueByLabel(string fieldLabel, int flyoutTimeoutSeconds = 15)
        {
            driver.SwitchTo().DefaultContent();

            var valueText = TryGetHeaderFieldValueByLabel(fieldLabel);

            if (valueText != null)
            {
                return valueText;
            }

            // Not found inline — likely collapsed into the "More Header Editable Fields" overflow flyout.
            var expandButton = GetActiveHeaderOverflowButton();

            if (expandButton != null)
            {
                var wasAlreadyExpanded = expandButton.GetAttribute("aria-expanded") == "true";

                if (!wasAlreadyExpanded)
                {
                    ClickViaJavaScript(expandButton);
                    driver.WaitForTransaction();
                }

                var deadline = DateTime.UtcNow.AddSeconds(flyoutTimeoutSeconds);

                while (DateTime.UtcNow < deadline && valueText == null)
                {
                    valueText = TryGetHeaderFieldValueByLabel(fieldLabel);

                    if (valueText == null)
                    {
                        Thread.Sleep(TimeSpan.FromMilliseconds(500));
                    }
                }

                CollapseHeaderFieldsFlyoutIfOpen();
            }

            return valueText;
        }

        /// <summary>
        /// Attempts to locate and read a header field's current value without expanding the overflow
        /// flyout, covering both known render shapes: readonly Fluent UI &lt;input&gt; controls
        /// (OptionSet fields) and plain lookup tag-text divs. Returns null if not currently present.
        /// </summary>
        private string TryGetHeaderFieldValueByLabel(string fieldLabel)
        {
            var containerXPath =
                $"//div[@data-control-name][.//label[normalize-space(text())='{fieldLabel}']]";

            var containers = driver.FindElements(By.XPath(containerXPath));

            if (containers.Count == 0)
            {
                return null;
            }

            var container = containers[^1];

            // OptionSet fields render as a readonly Fluent UI <input>, where the display value
            // lives in the 'value' attribute rather than as rendered text.
            var inputs = container.FindElements(By.XPath(".//input[@readonly]"));

            if (inputs.Count > 0)
            {
                return inputs[0].GetAttribute("value")?.Trim();
            }

            // Fallback: lookup/tag-style fields render the value as plain text in a nested div.
            var valueDivs = container.FindElements(By.XPath(".//div[@data-id[contains(.,'selected_tag_text')]]"));

            if (valueDivs.Count > 0)
            {
                return valueDivs[0].Text.Trim();
            }

            return null;
        }

        /// <summary>
        /// Returns the "More Header Editable Fields" overflow button for whichever header is
        /// currently active/topmost. When a modal dialog (e.g. a Work Order Task popup) is open over
        /// an underlying record page, multiple overflow buttons can exist in the DOM simultaneously —
        /// the background page's (now covered/non-interactable) and the modal's own. The active one
        /// is consistently the last in document order, since Dynamics appends dialog content after
        /// the underlying page markup.
        /// </summary>
        public IWebElement GetActiveHeaderOverflowButton()
        {
            var expandButtons = driver.FindElements(By.XPath(OverflowButtonXPath));
            return expandButtons.Count > 0 ? expandButtons[^1] : null;
        }

        /// <summary>
        /// Collapses the "More Header Editable Fields" overflow flyout if it is currently expanded,
        /// so subsequent header field lookups start from a known collapsed state rather than
        /// toggling an already-open flyout closed via a stray click.
        /// </summary>
        public void CollapseHeaderFieldsFlyoutIfOpen()
        {
            var expandButton = GetActiveHeaderOverflowButton();

            if (expandButton != null && expandButton.GetAttribute("aria-expanded") == "true")
            {
                ClickViaJavaScript(expandButton);
                driver.WaitForTransaction();
            }
        }

        /// <summary>
        /// Clicks an element via JavaScript rather than Selenium's native click, to avoid
        /// ElementClickInterceptedException when another element (e.g. a covered background page's
        /// control, or an inner icon/span) visually overlaps the target's click point.
        /// </summary>
        private void ClickViaJavaScript(IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
        }
    }
}