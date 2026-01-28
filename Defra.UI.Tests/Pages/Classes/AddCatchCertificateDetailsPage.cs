using Defra.UI.Framework.Driver;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public  class AddCatchCertificateDetailsPage : IAddCatchCertificateDetailsPage
    {
        private IObjectContainer _objectContainer;
        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement txtCatchCertificateReference => _driver.WaitForElement(By.Id("catch-certificate-reference-1"));
        private IWebElement addCatchCertificateText(string text) => _driver.FindElement(By.XPath($"//*[normalize-space(text())='{text}']"));
        private IWebElement dateOfIssue(string date) => _driver.FindElement(By.XPath($"//input[contains(@id, 'date-of-issue-{date}')]"));
        private IWebElement imgCalendar => _driver.FindElement(By.XPath("//*[@id=\"date-of-issue-1\"]/div[4]/button/span"));
        private IWebElement txtFlagStateOfCatchingVessel => _driver.FindElement(By.XPath("//input[@id='flag-state-1']"));
        private IWebElement selectAll => _driver.FindElement(By.XPath("//*[@id=\"hidden-select-all-checkbox-container-1\"]/div/label"));
        private IWebElement selectSpecies(string position) => _driver.FindElement(By.XPath($"(//*[contains(@id,'hidden-species')]/div/input)[{position}]"));
        #endregion
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public AddCatchCertificateDetailsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded(string pageTitle)
        {
            return primaryTitle.Text.Trim().Equals(pageTitle, StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifyContent(string content)
        {
            return addCatchCertificateText(content).Text.Equals(content);
        }
        
        public bool VerifyCalendar()
        {
            return imgCalendar.Text.Equals("Choose date");
        }
        
        public void EnterCatchCertificateReference(string reference)
        {
            txtCatchCertificateReference.SendKeys(reference);
        }
        
        public void EnterDateOfIssue(string day,string month,string year)
        {
            dateOfIssue("day").SendKeys(day);
            dateOfIssue("month").SendKeys(month);
            dateOfIssue("year").SendKeys(year);
        }
        
        public void EnterFlagStateOfCatchingVessel(string FlagState)
        {
            txtFlagStateOfCatchingVessel.Click();
            txtFlagStateOfCatchingVessel.Clear();
            txtFlagStateOfCatchingVessel.SendKeys($"{FlagState}{Keys.ArrowDown}{Keys.Enter}");
        }

        public void SelectSpecies(string species)
        {
            selectSpecies(species).Click();
        }

    }
}
