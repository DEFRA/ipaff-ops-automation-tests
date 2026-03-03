using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using Faker;
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
        private IWebElement lnkSearchForApproved => _driver.FindElement(By.Name("add-establishment"));
        private IWebElement countryDropdown => _driver.WaitForElement(By.Id("establishment-country-code"));
        private IWebElement typeDropdown => _driver.FindElement(By.Id("establishment-type"));
        private IWebElement statusDropdown => _driver.FindElement(By.Id("establishment-status"));
        private IReadOnlyCollection<IWebElement> lnkSelectEstablishment => _driver.FindElements(By.Id("select-establishment-1"));
        private IWebElement establishmentSearchResultFirstName => _driver.FindElement(By.XPath("//*[@id='establishments-search-results-row-1']/td[1]"));
        private IReadOnlyCollection<IWebElement> establishmentSearchResultTable => _driver.FindElements(By.XPath("//*[@id='establishments-table']/tbody"));
        private IWebElement selectedEstablishment => _driver.WaitForElement(By.XPath("//*[@id='establishments-row-1']/td[1]"));
        private IWebElement txtapprovedEstablishmentCountry => _driver.FindElement(By.XPath("//*[@id='establishments-row-1']/td[2]"));
        private IWebElement txtapprovedEstablishmentType => _driver.FindElement(By.XPath("//*[@id='establishments-row-1']/td[3]"));
        private IWebElement txtapprovedEstablishmentApprovalNum => _driver.FindElement(By.XPath("//*[@id='establishments-row-1']/td[4]"));
        private IWebElement selectedEstablishment2 => _driver.FindElement(By.XPath("//*[@id='establishments-row-2']/td[1]"));
        private IWebElement txtapprovedEstablishmentCountry2 => _driver.FindElement(By.XPath("//*[@id='establishments-row-2']/td[2]"));
        private IWebElement txtapprovedEstablishmentType2 => _driver.FindElement(By.XPath("//*[@id='establishments-row-2']/td[3]"));
        private IWebElement txtapprovedEstablishmentApprovalNum2 => _driver.FindElement(By.XPath("//*[@id='establishments-row-2']/td[4]"));
        private IWebElement lnkRemoveEstablishment => _driver.FindElement(By.Id("establishment-remove-1"));
        private IReadOnlyCollection<IWebElement> lstCountryInSearchResult => _driver.FindElements(By.XPath("//*[@id='establishments-search-results']//td[6]"));
        private IReadOnlyCollection<IWebElement> lstTypeInSearchResult => _driver.FindElements(By.XPath("//*[@id='establishments-search-results']//td[3]"));
        private IReadOnlyCollection<IWebElement> lstStatusInSearchResult => _driver.FindElements(By.XPath("//*[@id='establishments-search-results']//td[5]"));
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
            lnkSelectEstablishment.FirstOrDefault().Click();
        }

        public bool VerifySelectedEstablismentName(string establishmentListFirstName)
        {
            return selectedEstablishment.Text.Trim().Equals(establishmentListFirstName);
        }

        public string GetSelectedEstablishmentName()
        {
            return establishmentSearchResultTable.Count == 2
               ? selectedEstablishment2.Text.Trim()
               : selectedEstablishment.Text.Trim();
        }

        public string GetSelectedEstablishmentCountry()
        {
            if (establishmentSearchResultTable.Count == 2)
                return txtapprovedEstablishmentCountry2.Text.Trim();
            else
                return txtapprovedEstablishmentCountry.Text.Trim();
        }

        public string GetSelectedEstablishmentType()
        {
            if (establishmentSearchResultTable.Count == 2)
                return txtapprovedEstablishmentType2.Text.Trim();
            else
                return txtapprovedEstablishmentType.Text.Trim();
        }

        public string GetSelectedEstablishmentApprovalNumber()
        {
            if (establishmentSearchResultTable.Count == 2)
                return txtapprovedEstablishmentApprovalNum2.Text.Trim();
            else
                return txtapprovedEstablishmentApprovalNum.Text.Trim();
        }

        public void ClickRemoveEstablishment()
        {
            lnkRemoveEstablishment.Click();
        }

        public bool VerifySelectedCountryOnlyDisplayed(string country)
        {               
            return lstCountryInSearchResult.All(x=>x.Text.Trim().Equals(country));
        }

        public bool VerifySelectedTypeOnlyDisplayed(string type)
        {
            return lstTypeInSearchResult.All(x => x.Text.Trim().Contains(type));
        }

        public bool VerifySelectedStatusOnlyDisplayed(string status)
        {
            return lstStatusInSearchResult.All(x => x.Text.Trim().Equals(status));
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