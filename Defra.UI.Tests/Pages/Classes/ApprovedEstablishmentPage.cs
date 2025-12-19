using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ApprovedEstablishmentPage : IApprovedEstablishmentPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement lnkSearchForApproved => _driver.WaitForElement(By.Name("add-establishment"));
        private IWebElement countryDropdown => _driver.FindElement(By.Id("establishment-country-code"));
        private IWebElement typeDropdown => _driver.FindElement(By.Id("establishment-type"));
        private IWebElement statusDropdown => _driver.FindElement(By.Id("establishment-status"));
        private IWebElement lnkSelectEstablishment => _driver.WaitForElement(By.Id("select-establishment-1"));
        private IWebElement establishmentSearchResultFirstName => _driver.WaitForElement(By.XPath("//*[@id='establishments-search-results-row-1']/td[1]"));
        private IWebElement selectedEstablishment => _driver.WaitForElement(By.XPath("//*[@id='establishments-row-1']/td[1]"));
        private IWebElement txtapprovedEstablishmentCountry => _driver.WaitForElement(By.XPath("//*[@id='establishments-row-1']/td[2]"));
        private IWebElement txtapprovedEstablishmentType => _driver.WaitForElement(By.XPath("//*[@id='establishments-row-1']/td[3]"));
        private IWebElement txtapprovedEstablishmentApprovalNum => _driver.WaitForElement(By.XPath("//*[@id='establishments-row-1']/td[4]"));
        private IWebElement lnkRemoveEstablishment => _driver.WaitForElement(By.Id("establishment-remove-1"));
        private IReadOnlyCollection<IWebElement> txtCountryInSearchResult => _driver.FindElements(By.XPath("//*[@id='establishments-search-results']//td[6]"));
        private IReadOnlyCollection<IWebElement> txtTypeInSearchResult => _driver.FindElements(By.XPath("//*[@id='establishments-search-results']//td[3]"));
        private IReadOnlyCollection<IWebElement> txtStatusInSearchResult => _driver.FindElements(By.XPath("//*[@id='establishments-search-results']//td[5]"));
        private IWebElement btnSearch => _driver.FindElement(By.Id("search"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ApprovedEstablishmentPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Documents")
                && primaryTitle.Text.Contains("Approved establishment of origin (where required)");
        }
       
        public void ClickSearchForApproved() 
        { 
            lnkSearchForApproved.Click(); 
        }

        public bool VerifySelectedCountryOfOrigin(string country)
        {
            var select = new SelectElement(countryDropdown);
            string actual = select.SelectedOption.Text.Trim();
            return actual.Contains(country);
        }

        public string GetEstablishmentListFirstName()
        {
            return establishmentSearchResultFirstName.Text.Trim();
        }

        public void ClickSelectEstablishment() 
        { 
            lnkSelectEstablishment.Click(); 
        }

        public bool VerifySelectedEstablismentName(string establishmentListFirstName)
        {
            return selectedEstablishment.Text.Trim().Equals(establishmentListFirstName);
        }

        public string GetSubtotalNetWeight()
        {
            return selectedEstablishment.Text.Trim();
        }

        public string GetSubtotalPackages()
        {
            return txtapprovedEstablishmentCountry.Text.Trim();
        }

        public string GetTotalNetWeight()
        {
            return txtapprovedEstablishmentType.Text.Trim();
        }

        public string GetTotalPackages()
        {
            return txtapprovedEstablishmentApprovalNum.Text.Trim();
        }

        public void ClickRemoveEstablishment()
        {
            lnkRemoveEstablishment.Click();
        }

        public bool VerifySelectedCountryOnlyDisplayed(string country)
        {
            foreach (var item in txtCountryInSearchResult)
            {
                 return item.Text.Trim().Equals(country); 
            }
            return false;
        }

        public bool VerifySelectedTypeOnlyDisplayed(string type)
        {
            foreach (var item in txtTypeInSearchResult)
            {
                 return item.Text.Trim().Equals(type);
            }
            return false;
        }

        public bool VerifySelectedStatusOnlyDisplayed(string status)
        {
            foreach (var item in txtStatusInSearchResult)
            {
                 return item.Text.Trim().Equals(status);
            }
            return false;
        }

        public void SelectTypeFromDropdown(string type)
        {
            new SelectElement(typeDropdown).SelectByText(type);
        }

        public void SelectStatusFromDropdown(string status)
        {
            new SelectElement(statusDropdown).SelectByText(status);
        }

        public void ClickSearchButton()
        {
            btnSearch.Click();
        }
    }
}