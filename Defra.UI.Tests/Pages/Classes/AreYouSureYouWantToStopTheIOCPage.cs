using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class AreYouSureYouWantToStopTheIOCPage : IAreYouSureYouWantToStopTheIOCPage
    {
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageHeading => _driver.WaitForElement(By.XPath("//h2[contains(@class,'govuk-heading-xl govuk-!-margin-bottom-6')]"), true);
        private IWebElement btnSubmit => _driver.FindElement(By.Id("submit-button"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public AreYouSureYouWantToStopTheIOCPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return pageHeading.Text.Contains("Are you sure you want to stop the intensified official control?");
        }

        public void ClickYesStopTheIntensifiedOfficialControl()
        {
            btnSubmit.Click();
        }
    }
}