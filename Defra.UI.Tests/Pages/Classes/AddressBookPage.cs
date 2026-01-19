using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class AddressBookPage : IAddressBookPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;
        private const int MaxRetryAttempts = 3;
        private const int RetryDelayMilliseconds = 1000;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement ddlType => _driver.FindElement(By.Id("type"));
        private IWebElement btnSearch => _driver.FindElement(By.Id("search"));
        private IWebElement economicOperatorsTable => _driver.WaitForElement(By.Id("economic-operators-table"));
        private IWebElement lnkAddAnAddress => _driver.FindElement(By.LinkText("Add an address"));
        private IWebElement lnkReturnToAddressBook => _driver.FindElement(By.LinkText("Return to Address Book"));
        private IWebElement lnkDashboard => _driver.FindElement(By.LinkText("Dashboard"));
        private IWebElement GetOperatorNameElement(string operatorName) =>
            _driver.FindElement(By.XPath($"//table[@id='economic-operators-table']//td[@class='govuk-table__cell' and normalize-space()='{operatorName}']"));

        private IWebElement GetOperatorTypeElement(string operatorName) =>
            _driver.FindElement(By.XPath($"//table[@id='economic-operators-table']//td[@class='govuk-table__cell' and normalize-space()='{operatorName}']/following-sibling::td[1]"));

        private IWebElement GetOperatorAddressElement(string operatorName) =>
            _driver.FindElement(By.XPath($"//table[@id='economic-operators-table']//td[@class='govuk-table__cell' and normalize-space()='{operatorName}']/following-sibling::td[2]"));

        private IWebElement GetOperatorCountryElement(string operatorName) =>
            _driver.FindElement(By.XPath($"//table[@id='economic-operators-table']//td[@class='govuk-table__cell' and normalize-space()='{operatorName}']/following-sibling::td[3]"));

        private IWebElement GetOperatorViewLink(string operatorName) =>
            _driver.FindElement(By.XPath($"//table[@id='economic-operators-table']//td[@class='govuk-table__cell' and normalize-space()='{operatorName}']/following-sibling::td[4]//a[text()='View']"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public AddressBookPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        #region Methods
        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Address book");
        }

        public void ClickAddAnAddress()
        {
            lnkAddAnAddress.Click();
        }

        public void SelectType(string type)
        {
            new SelectElement(ddlType).SelectByText(type);
        }

        public void ClickSearchButton() => btnSearch.Click();

        public bool ValidateTypeInSearchResults(string type)
        {
            var rows = economicOperatorsTable.FindElements(By.XPath("//tbody/tr")).Count;
            if (rows == 0)
                return false;

            var matchedRows = economicOperatorsTable.FindElements(By.XPath("//tbody/tr[td[2][contains(normalize-space(.), " + $"{type}" + ")]]")).Count;

            return matchedRows == rows;
        }

        public bool IsOperatorDisplayedInAddressBook(string operatorName, string operatorType, string operatorAddress, string operatorCountry)
        {
            for (int attempt = 1; attempt <= MaxRetryAttempts; attempt++)
            {
                try
                {
                    var nameElement = GetOperatorNameElement(operatorName);
                    var typeElement = GetOperatorTypeElement(operatorName);
                    var addressElement = GetOperatorAddressElement(operatorName);
                    var countryElement = GetOperatorCountryElement(operatorName);

                    // Verify the operator name is displayed
                    var nameDisplayed = nameElement.Displayed && nameElement.Text.Trim().Equals(operatorName);

                    // Verify the operator type matches (case-insensitive comparison)
                    var typeDisplayed = typeElement.Displayed &&
                                       typeElement.Text.Trim().Equals(operatorType, StringComparison.OrdinalIgnoreCase);

                    // Verify the operator address contains the expected address
                    var addressDisplayed = addressElement.Displayed &&
                                          addressElement.Text.Trim().Contains(operatorAddress, StringComparison.OrdinalIgnoreCase);

                    // Verify the operator country matches
                    var countryDisplayed = countryElement.Displayed &&
                                          countryElement.Text.Trim().Equals(operatorCountry, StringComparison.OrdinalIgnoreCase);

                    return nameDisplayed && typeDisplayed && addressDisplayed && countryDisplayed;
                }
                catch (Exception ex)
                {
                    // Log the attempt failure
                    Console.WriteLine($"Attempt {attempt} of {MaxRetryAttempts} failed to find operator '{operatorName}'. Exception: {ex.Message}");

                    // If this is not the last attempt, refresh the page and retry
                    if (attempt < MaxRetryAttempts)
                    {
                        Console.WriteLine($"Refreshing the Address Book page and retrying...");
                        _driver.Navigate().Refresh();

                        // Wait for the page to reload by verifying it's loaded
                        if (!IsPageLoaded())
                        {
                            Console.WriteLine($"Warning: Page did not reload properly on attempt {attempt}");
                        }

                        Thread.Sleep(RetryDelayMilliseconds);
                    }
                    else
                    {
                        // Last attempt failed, return false
                        Console.WriteLine($"All {MaxRetryAttempts} attempts exhausted. Operator '{operatorName}' not found in address book.");
                        return false;
                    }
                }
            }

                return false;
        }

        public string GetOperatorName(string operatorName) => GetOperatorNameElement(operatorName).Text.Trim();
        public string GetOperatorType(string operatorName) => GetOperatorTypeElement(operatorName).Text.Trim();
        public string GetOperatorAddress(string operatorName) => GetOperatorAddressElement(operatorName).Text.Trim();
        public string GetOperatorCountry(string operatorName) => GetOperatorCountryElement(operatorName).Text.Trim();

        public void ClickDashboardLink()
        {
            lnkDashboard.Click();
        }

        public void ClickViewOperator(string operatorName)
        {
            GetOperatorViewLink(operatorName).Click();
        }
        #endregion
    }
}
