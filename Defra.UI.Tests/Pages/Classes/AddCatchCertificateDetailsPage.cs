using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class AddCatchCertificateDetailsPage : IAddCatchCertificateDetailsPage
    {
        private IObjectContainer _objectContainer;
        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement setCatchCertificateReference(int index) => _driver.WaitForElement(By.Id($"catch-certificate-reference-{index}"));
        private IWebElement addCatchCertificateText(string text) => _driver.FindElement(By.XPath($"//*[normalize-space(text())='{text}']"));
        private IWebElement dateOfIssue(string date, int index) => _driver.FindElement(By.XPath($"//input[contains(@id, 'date-of-issue-{date}-{index}')]"));
        private IWebElement imgCalendar => _driver.FindElement(By.XPath("//*[@id=\"date-of-issue-1\"]/div[4]/button/span"));
        private IWebElement setFlagStateOfCatchingVessel(int index) => _driver.FindElement(By.XPath($"//input[@id='flag-state-{index}']"));
        private IWebElement selectAll => _driver.FindElement(By.XPath("//*[@id=\"hidden-select-all-checkbox-container-1\"]/div/label"));
        private IWebElement selectSpecies(int position) => _driver.FindElement(By.XPath($"(//*[contains(@id,'hidden-species')]/div/input)[{position}]"));
        private IWebElement lnkChange => _driver.FindElement(By.XPath("//span[@class='govuk-details__summary-text']"));
        private IWebElement txtNoOfCatchCertificates => _driver.FindElement(By.Id("number-of-catch-certificates"));
        private IWebElement lstNoOfCatchCertificatRefereceSections => _driver.FindElement(By.Id("catch-certificate-details-3"));
        private IWebElement btnUpdate => _driver.FindElement(By.Id("update-number-of-catch-certificates"));
        private IWebElement setUpdateDetails(int index) => _driver.FindElement(By.Id($"update-catch-certificate-details-{index}"));
        private IWebElement lnkSaveAndReturnToManageCertificate => _driver.FindElement(By.Id("save-and-return-manage-catch-certificates"));
        private IReadOnlyCollection<IWebElement> speciesDetailsList(int index) => _driver.FindElements(By.XPath($"(//div[contains(@id,'checkbox-Metanephrops challengeri')])[{index}]//p"));
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

        public void EnterCatchCertificateReference(string reference, int index=1)
        {
            setCatchCertificateReference(index).SendKeys(reference);
        }

        public void EnterDateOfIssue(string day, string month, string year, int index = 1)
        {
            dateOfIssue("day", index).SendKeys(day);
            dateOfIssue("month", index).SendKeys(month);
            dateOfIssue("year", index).SendKeys(year);
        }

        public void EnterFlagStateOfCatchingVessel(string FlagState, int index=1)
        {
            setFlagStateOfCatchingVessel(index).Clear();
            setFlagStateOfCatchingVessel(index).SendKeys($"{FlagState}{Keys.ArrowDown}{Keys.Enter}");
        }

        public void SelectSpecies(int species)
        {
            selectSpecies(species).Click();
        }

        public void ClickChangeLink()
        {
            lnkChange.Click();
        }

        public void EnterNumberOfCatchCertificates(string noOfCertificateRef)
        {
            txtNoOfCatchCertificates.Clear();
            txtNoOfCatchCertificates.SendKeys(noOfCertificateRef.ToString());
        }

        public bool VerifyNoOfCatchReferenceSections(int numberOfRefBlocks)
        {
            return lstNoOfCatchCertificatRefereceSections.Displayed;
        }

        public void ClickUpdate()
        {
            btnUpdate.Click();
        }

        public void ClickUpdate(int index)
        {
            setUpdateDetails(index).Click();
        }

        public void ClickSaveAndReturnToManageCertificateLink()
        {
            lnkSaveAndReturnToManageCertificate.Click();
        }

        public string[] GetSpeciesDetailsList(int index)
        {
            return [.. speciesDetailsList(index).Select(e => e.Text.Trim())];
        }
    }
}
