using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class AreYouSureYouWantToCreateTheIOCPage : IAreYouSureYouWantToCreateTheIOCPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h2[contains(@class,'govuk-heading-xl')]"), true);
        private IWebElement btnYesCreateIntensifiedOfficialControl => _driver.FindElement(By.Id("submit-button"));
        private IWebElement lnkNoDontCreateIntensifiedOfficialControl => _driver.FindElement(By.XPath("//a[contains(@class,'govuk-link') and contains(normalize-space(),'No')]"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public AreYouSureYouWantToCreateTheIOCPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        #region Methods

        public bool IsPageLoaded()
        {
            var normalisedText = NormaliseText(pageTitle.Text);
            return normalisedText.Equals(
                "Are you sure you want to create the intensified official control?",
                StringComparison.OrdinalIgnoreCase);
        }

        public bool IsYesCreateButtonDisplayed()
        {
            var normalisedText = NormaliseText(btnYesCreateIntensifiedOfficialControl.Text);
            return btnYesCreateIntensifiedOfficialControl.Displayed &&
                   normalisedText.Equals(
                       "Yes, create the intensified official control",
                       StringComparison.OrdinalIgnoreCase);
        }

        public bool IsNoDontCreateLinkDisplayed()
        {
            var normalisedText = NormaliseText(lnkNoDontCreateIntensifiedOfficialControl.Text);
            return lnkNoDontCreateIntensifiedOfficialControl.Displayed &&
                   normalisedText.Equals(
                       "No, don't create the intensified official control",
                       StringComparison.OrdinalIgnoreCase);
        }

        public void ClickYesCreateIntensifiedOfficialControl()
        {
            btnYesCreateIntensifiedOfficialControl.Click();
        }

        private static string NormaliseText(string text) =>
            System.Text.RegularExpressions.Regex.Replace(text.Trim(), @"\s+", " ");

        #endregion
    }
}