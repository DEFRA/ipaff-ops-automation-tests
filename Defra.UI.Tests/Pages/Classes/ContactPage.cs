using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ContactPage : IContactPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.ClassName("govuk-heading-l"), true);
        private IWebElement txtContact => _driver.FindElement(By.XPath("//h2[@class='govuk-heading-l']/following-sibling::p"));
        private IWebElement lnkBack => _driver.FindElement(By.Id("back-link"));

        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ContactPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return pageTitle.Text.Contains("Contacting us by phone or by email");
        }

        public string GetAphaContactText => txtContact?.Text?.Trim() ?? string.Empty;

        public void ClickBackLink() => lnkBack.Click();
    }
}