using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterWhoIsTheContactForThisApplicationPage : IExporterWhoIsTheContactForThisApplicationPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Who is the contact for this application?']"), true);
        private IWebElement txtOrganisationName => _driver.WaitForElement(By.Id("inspection-organisation-name"), true);
        private IWebElement txtContactName => _driver.WaitForElement(By.Id("inspection-contact-name"), true);
        private IWebElement txtPhoneNumber => _driver.WaitForElement(By.Id("inspection-contact-phone-number"), true);
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("Button-SaveAndContinue"));
        #endregion

        public ExporterWhoIsTheContactForThisApplicationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Who is the contact for this application?");

        public void EnterContactDetails(string organisationName, string contactName, string telephone)
        {
            txtOrganisationName.Clear();
            txtOrganisationName.SendKeys(organisationName);

            txtContactName.Clear();
            txtContactName.SendKeys(contactName);

            txtPhoneNumber.Clear();
            txtPhoneNumber.SendKeys(telephone);
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();
    }
}