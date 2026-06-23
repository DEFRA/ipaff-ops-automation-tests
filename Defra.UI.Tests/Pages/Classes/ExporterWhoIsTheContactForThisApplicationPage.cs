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
        private IWebElement txtName => _driver.WaitForElement(By.XPath("//input[contains(@id,'name') and @type='text'][1]"), true);
        private IWebElement txtEmail => _driver.WaitForElement(By.XPath("//input[contains(@id,'email') or @type='email'][1]"), true);
        private IWebElement txtTelephone => _driver.WaitForElement(By.XPath("//input[contains(@id,'telephone') or contains(@id,'phone')][1]"), true);
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.XPath("//button[normalize-space()='Save and continue'] | //input[@value='Save and continue']"));
        #endregion

        public ExporterWhoIsTheContactForThisApplicationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Who is the contact for this application?");

        public void EnterContactDetails(string name, string email, string telephone)
        {
            txtName.Clear();
            txtName.SendKeys(name);
            txtEmail.Clear();
            txtEmail.SendKeys(email);
            txtTelephone.Clear();
            txtTelephone.SendKeys(telephone);
        }

        public void ClickSaveAndContinueButton() => btnSaveAndContinue.Click();
    }
}