using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;
using System.Text.RegularExpressions;

namespace Defra.UI.Tests.Pages.Classes
{
    public class YourIOCHasBeenPutInPlacePage : IYourIOCHasBeenPutInPlacePage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h2[contains(@class,'govuk-panel__title')]"), true);
        private IWebElement txtIOCNumber => _driver.FindElement(By.Id("rec-number"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        private static readonly Regex IocNumberFormat = new(@"^IOC\.\d{4}\.\d+$", RegexOptions.Compiled);

        public YourIOCHasBeenPutInPlacePage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        #region Methods

        public bool IsPageLoaded()
        {
            return pageTitle.Text.Contains("Your intensified official control has been put in place");
        }

        public string GetIntensifiedOfficialControlNumber()
        {
            return txtIOCNumber.Text.Trim();
        }

        public bool IsIntensifiedOfficialControlNumberInCorrectFormat()
        {
            return IocNumberFormat.IsMatch(GetIntensifiedOfficialControlNumber());
        }

        #endregion
    }
}