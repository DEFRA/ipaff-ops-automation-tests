using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using SeleniumExtras.WaitHelpers;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CheckUploadedCommodityPage : ICheckUploadedCommodityPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement btnConfirmAndContinue => _driver.FindElement(By.Id("button-save-and-continue"));
        private IWebElement txtUploadSuccessBannerContent => _driver.FindElement(By.XPath("//*[@class='govuk-notification-banner__content']/p"));
        private IWebElement txtInfoMessageHeading => _driver.FindElement(By.XPath("//h2[@class='govuk-heading-s']"));
        private IWebElement txtInfoMessageContent => _driver.FindElement(By.XPath("//p[@class='govuk-body']"));
        private By lblUploading => By.XPath("//h3[text()='Uploading CSV...']");
        private By lblSuccessBanner => By.XPath("//h2[text()='Upload successful']");
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public CheckUploadedCommodityPage(IObjectContainer container)
        {
            _objectContainer = container;
        }


        public bool IsPageLoaded()
        {
            return pageTitle.Text.Contains("Check uploaded commodity details");
        }


        public bool WaitForUploadToCompleteAndVerifySuccessMessage(String successsMessage)
        {
            WebDriverWait _wait = new WebDriverWait(_driver, TimeSpan.FromMinutes(4));
            try
            {
                // Wait for "uploading" to disappear
                _wait.Until(d =>
                {
                    try { return !d.FindElement(lblUploading).Displayed; }
                    catch(NoSuchElementException) { return true; }
                });

                // Wait for success message
                _wait.Until(ExpectedConditions.ElementIsVisible(lblSuccessBanner));
                return _driver.FindElement(lblSuccessBanner).Text.Equals(successsMessage);
            }
            catch
            {
                return false; // timeout or failure
            }
        }


        public bool IsUploadSuccessMsgDisplayed(string successMsg)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(60));

            try
            {
                return wait.Until(driver =>
                {
                    var elem = driver.FindElements(By.Id("upload-banner")).FirstOrDefault();
                    if (elem == null)
                        return false;

                    var text = elem.Text?.Trim() ?? string.Empty;
                    return text.Contains(successMsg, StringComparison.OrdinalIgnoreCase);
                });
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public bool IsCountOfCommodityMatchesWithInput(int expectedCommodityCount)
        {
            string bannerText = txtUploadSuccessBannerContent.Text.Trim();
            Console.WriteLine("bannerText " + bannerText);

            int actualCount = 0;

            var match = System.Text.RegularExpressions.Regex.Match(bannerText, @"(\d+)\s+commodity\s+lines\s+added");

            Console.WriteLine("match " + match);

            if (match.Success)
            {
                actualCount = int.Parse(match.Groups[1].Value);

                Console.WriteLine("actualCount " + actualCount);
            }
            else
            {
                throw new Exception("");
            }

            Console.WriteLine("expectedCommodityCount " + expectedCommodityCount);
            return actualCount == expectedCommodityCount;
        }

        public void ValidateAllCommodityDetails(Reqnroll.Table inputAllCommodityData,
                                        ref bool allDataMatches, List<string> mismatches)
        {
            int rowIndex = 0;
            bool morePages = false;

            do
            {
                // LOAD CURRENT PAGE TABLES
                var tables = _driver.FindElements(By.XPath("//*[@class='commodity-summary']"));
                Console.WriteLine($"UI tables found on this page = {tables.Count}");


                // VALIDATE EACH TABLE AGAINST ONE INPUT ROW
                int tableIndex = 0;
                foreach (var table in tables)
                {
                    if (rowIndex >= inputAllCommodityData.Rows.Count)
                        break;


                    // ⭐ READ‑ONLY VALIDATION — check for editable elements
                    var editable = table.FindElements(By.XPath(".//input | .//select | .//textarea"));
                    if (editable.Count > 0)
                    {
                        allDataMatches = false;
                        mismatches.Add($"Row {rowIndex + 1}: Table is NOT read‑only. Found {editable.Count} editable elements.");
                        Console.WriteLine($"[ReadOnlyCheck] Row {rowIndex + 1}: ❌ NOT read‑only! Found {editable.Count} edit controls.");
                    }
                    else
                    {
                        Console.WriteLine($"[ReadOnlyCheck] Row {rowIndex + 1}: ✔ Read‑only");
                    }

                    try
                    {
                        ValidateSingleRow(inputAllCommodityData, table, rowIndex, ref allDataMatches, mismatches);
                        rowIndex++;
                        tableIndex++;
                    }
                    catch (Exception ex)
                    {
                        allDataMatches = false;
                        mismatches.Add($"Exception at row {rowIndex + 1}: {ex.Message}");
                        rowIndex++;
                        tableIndex++;
                    }
                }



                // PAGINATION: CLICK NEXT BUTTON IF EXISTS
                var nextButtons = _driver.FindElements(By.Id("next-page"));

                if (nextButtons.Count > 0)
                {
                    Console.WriteLine("Clicking NEXT PAGE...");

                    // Capture "Showing ... results" text BEFORE clicking
                    var beforeShowing = _driver.FindElements(
                        By.XPath("//p[contains(.,'Showing') and contains(.,'results')]"))
                        .FirstOrDefault()?.Text ?? string.Empty;


                    nextButtons[0].Click();


                    // WAIT UNTIL the "Showing ... results" text CHANGES
                    Wait.Until(TimeSpan.FromSeconds(10), () =>
                    {
                        var afterShowing = _driver.FindElements(
                            By.XPath("//p[contains(.,'Showing') and contains(.,'results')]"))
                            .FirstOrDefault()?.Text ?? string.Empty;


                        // Change in text means page finished loading
                        return !string.Equals(afterShowing, beforeShowing, StringComparison.Ordinal);
                    });

                    morePages = true;
                }
                else
                {
                    Console.WriteLine("No more pages.");
                    morePages = false;
                }

            } while (morePages);
        }


        private void ValidateSingleRow(Reqnroll.Table input, IWebElement table, int rowIndex,
                                        ref bool allDataMatches, List<string> mismatches)
        {
            bool vcLogged = false;

            foreach (var column in input.Header)
            {
                string expectedValue = string.Empty;
                string actualValue = string.Empty;

                // ===================== VARIETY + CLASS =====================
                if (column == "Variety" || column == "Class")
                {
                    string variety = input.Rows[rowIndex]["Variety"]?.Trim();
                    string classValue = input.Rows[rowIndex]["Class"]?.Trim();

                    bool emptyInput = string.IsNullOrEmpty(variety) && string.IsNullOrEmpty(classValue);

                    string expectedVC = "";
                    if (!emptyInput)
                    {
                        if (string.IsNullOrEmpty(variety))
                            expectedVC = classValue;
                        else if (string.IsNullOrEmpty(classValue))
                            expectedVC = variety;
                        else
                            expectedVC = $"{variety}, {classValue}";
                    }

                    var vcElement = table.FindElements(By.XPath(".//div[@id='variety-and-class']/div[2]/span"));
                    bool uiHasVC = vcElement.Count > 0;

                    if (!vcLogged)
                    {
                        Console.WriteLine($"Row {rowIndex + 1}: Expected VC='{expectedVC}', UIHasVC={uiHasVC}");
                        if (uiHasVC)
                        {
                            Console.WriteLine($"Row {rowIndex + 1}: Actual VC='{vcElement[0].Text.Trim()}'");
                        }
                        vcLogged = true;
                    }

                    if (emptyInput && !uiHasVC)
                        continue;

                    if (!emptyInput && !uiHasVC)
                    {
                        allDataMatches = false;
                        mismatches.Add($"Variety/Class missing in UI for row {rowIndex + 1}");
                        continue;
                    }

                    string actualVC = vcElement[0].Text.Trim();

                    if (!string.Equals(actualVC, expectedVC, StringComparison.OrdinalIgnoreCase))
                    {
                        allDataMatches = false;
                        mismatches.Add($"Variety/Class mismatch at row {rowIndex + 1}: expected '{expectedVC}', actual '{actualVC}'");
                    }

                    continue;
                }
                // ===========================================================



                // Normal expected value
                expectedValue = input.Rows[rowIndex][column]?.Trim();

                if (string.IsNullOrEmpty(expectedValue) &&
                    column == "Intended for final users (or commercial flower production)")
                    continue;

                if (column == "Controlled atmosphere container" &&
                    string.IsNullOrEmpty(expectedValue))
                    expectedValue = "No";

                if (column == "Quantity" || column == "Net weight (kg)")
                    expectedValue = NormalizeDecimal(expectedValue);

                if (column == "Type of package")
                    expectedValue = GetPackageTypeFullForm(expectedValue);

                if (column == "Quantity type")
                    expectedValue = GetQuantityTypeFullForm(expectedValue);


                actualValue = GetColumnValueFromTable(table, column, rowIndex)?.Trim() ?? "";

                Console.WriteLine($"[Actual] Page validation | Row {rowIndex + 1} | Column '{column}' | Actual='{actualValue}'");


                //⭐ ORDER CHECK (actual comparison already done by your mismatch logic)
                if (column.Equals("Commodity code", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"[OrderCheck] Row {rowIndex + 1}: Expected='{expectedValue}', Actual='{actualValue}'");
                }



                if (!string.Equals(actualValue, expectedValue, StringComparison.OrdinalIgnoreCase))
                {
                    allDataMatches = false;
                    mismatches.Add($"{column} mismatch at row {rowIndex + 1}: expected '{expectedValue}', actual '{actualValue}'");
                }
            }
        }


        private string GetColumnValueFromTable(IWebElement table, string columnName, int rowIndex)
        {
            IReadOnlyCollection<IWebElement> cells;

            switch (columnName)
            {
                case "Commodity code":
                    cells = table.FindElements(By.XPath(
                       ".//div[contains(@class,'govuk-grid-row')][.//span[normalize-space()='Commodity code']]//div[contains(@class,'govuk-grid-column-one-half')][2]/span"
                    ));
                    break;

                case "EPPO code":
                    cells = table.FindElements(By.XPath(
                       ".//div[contains(@class,'govuk-grid-row')][.//span[normalize-space()='EPPO']]//div[contains(@class,'govuk-grid-column-one-half')][2]/span"
                    ));
                    break;

                case "Genus and Species":
                    cells = table.FindElements(By.XPath(
                       ".//div[contains(@class,'govuk-grid-row')][.//span[normalize-space()='Genus and species']]//div[contains(@class,'govuk-grid-column-one-half')][2]/span"
                    ));
                    break;

                case "Variety and class":
                case "Variety":
                case "Class":
                    cells = table.FindElements(By.XPath(".//div[@id='variety-and-class']/div[2]/span"));
                    break;

                case "Number of packages":
                    cells = table.FindElements(By.XPath(
                       ".//div[contains(@class,'govuk-grid-row')][.//span[normalize-space()='Packages']]//div[contains(@class,'govuk-grid-column-one-half')][2]/span"
                    ));
                    break;

                case "Type of package":
                    cells = table.FindElements(By.XPath(
                       ".//div[contains(@class,'govuk-grid-row')][.//span[normalize-space()='Package type']]//div[contains(@class,'govuk-grid-column-one-half')][2]/span"
                    ));
                    break;

                case "Quantity":
                    cells = table.FindElements(By.XPath(
                       ".//div[contains(@class,'govuk-grid-row')][.//span[normalize-space()='Quantity']]//div[contains(@class,'govuk-grid-column-one-half')][2]/span"
                    ));
                    break;

                case "Quantity type":
                    cells = table.FindElements(By.XPath(
                       ".//div[contains(@class,'govuk-grid-row')][.//span[normalize-space()='Quantity type']]//div[contains(@class,'govuk-grid-column-one-half')][2]/span"
                    ));
                    break;

                case "Net weight (kg)":
                    cells = table.FindElements(By.XPath(
                       ".//div[contains(@class,'govuk-grid-row')][.//span[normalize-space()='Net weight']]//div[contains(@class,'govuk-grid-column-one-half')][2]/span"
                    ));
                    break;

                case "Controlled atmosphere container":
                    cells = table.FindElements(By.XPath(
                       ".//div[contains(@class,'govuk-grid-row')][.//span[normalize-space()='Controlled atmosphere container']]//div[contains(@class,'govuk-grid-column-one-half')][2]/span"
                    ));
                    break;
                case "Intended for final users (or commercial flower production)":
                    cells = table.FindElements(By.XPath(
                       ".//div[contains(@class,'govuk-grid-row')][.//span[contains(normalize-space(),'Intended for final users')]]//div[contains(@class,'govuk-grid-column-one-half')][2]/span"
                    ));
                    break;

                default:
                    throw new ArgumentException($"Unknown column name: {columnName}");
            }


            if (cells == null || cells.Count == 0)
            {
                return string.Empty;
            }


            if (columnName.Equals("Variety", StringComparison.OrdinalIgnoreCase)
                || columnName.Equals("Class", StringComparison.OrdinalIgnoreCase)
                || columnName.Equals("Variety and class", StringComparison.OrdinalIgnoreCase))
            {
                var cell = cells.ElementAt(0);
                string actualValue = cell?.Text.Trim() ?? string.Empty;

                Console.WriteLine($"Returning actual value = '{actualValue}' for column = {columnName}, rowIndex = {rowIndex}");
                return actualValue;
            }

            var normalCell = cells.ElementAt(0);
            return ReadText(normalCell);
        }

        private string ReadText(IWebElement el)
        {
            if (el == null) return string.Empty;

            // 1) Try visible text
            var text = el.Text;
            if (!string.IsNullOrWhiteSpace(text))
                return text.Trim();


            // 2) Fallback for hidden elements (mobile rows)
            // Selenium 4: GetDomProperty is supported
            var innerText = el.GetDomProperty("innerText");
            if (!string.IsNullOrWhiteSpace(innerText))
                return innerText.Trim();

            var textContent = el.GetDomProperty("textContent") ?? el.GetAttribute("textContent");
            if (!string.IsNullOrWhiteSpace(textContent))
                return textContent.Trim();

            return string.Empty;
        }

        private string NormalizeDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            if (!decimal.TryParse(value, out decimal num))
                return value;

            string[] parts = value.Split('.');

            if (parts.Length == 1)
                return num.ToString("0.0");

            string decimals = parts[1];

            if (decimals.All(c => c == '0'))
                return parts[0] + ".0";

            return parts[0] + "." + decimals;
        }

        private string GetPackageTypeFullForm(string inputPackageType)
        {
            if (string.IsNullOrEmpty(inputPackageType))
                return inputPackageType;

            switch (inputPackageType.Trim().ToUpper())
            {
                case "BG": return "Bag";
                case "BL": return "Bale";
                case "BX": return "Box";
                case "VR": return "Bulk solid granular particles (\"grains\")";
                case "CA": return "Can";
                case "CT": return "Carton";
                case "CS": return "Case";
                case "CK": return "Cask";
                case "CF": return "Coffer";
                case "CN": return "Container";
                case "CR": return "Crate";
                case "PK": return "Package";
                case "PX": return "Pallet";
                case "PU": return "Tray";
                case "TU": return "Tube";
                case "VI": return "Vial";
                case "BA": return "Wood barrel";
                case "BE": return "Wood bundle";
                case "EE": return "Wood case with pallet base";
                case "8B": return "Wood crate";
                default: return inputPackageType;
            }
        }

        private string GetQuantityTypeFullForm(string inputQuantityType)
        {
            if (string.IsNullOrEmpty(inputQuantityType))
                return inputQuantityType;

            switch (inputQuantityType.Trim().ToUpper())
            {
                case "BLB": return "Bulbs";
                case "CRZ": return "Corms and rhizomes";
                case "KGM": return "Kilograms";
                case "PCS": return "Pieces";
                case "PTC": return "Plants in tissue culture";
                case "SDS": return "Seeds";
                case "STM": return "Stems";
                default: return inputQuantityType;
            }
        }

        public void ClickConfirmAndContinueButton()
        {
            btnConfirmAndContinue.Click();
        }

        public bool VerifyInfoMessage(string msgHeading, string msgContent)
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView()", txtInfoMessageHeading);

            return txtInfoMessageHeading.Text.Trim().Contains(msgHeading)
                && txtInfoMessageContent.Text.Trim().Contains(msgContent);
        }
    }
}