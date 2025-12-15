using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ControlledDestinationPage : IControlledDestinationPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.ClassName("govuk-heading-xl"), true);
        private IWebElement lnkAddControlledDestination => _driver.FindElement(By.Id("add-controlled-destination"));
        private IWebElement verifyControlledDestinationName => _driver.FindElement(By.XPath("//td[@headers='controlled-destination-name']"));
        private IWebElement verifyControlledDestinationAddress => _driver.FindElement(By.XPath("//td[@headers='controlled-destination-address']"));
        private IWebElement verifyControlledDestinationType => _driver.FindElement(By.XPath("//td[@headers='controlled-destination-type']"));
        private IWebElement btnSaveAndContinue => _driver.FindElement(By.Id("button-save-and-continue"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ControlledDestinationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Select a controlled destination");
        }

        public void ClickAddControlledDestination()
        {
            lnkAddControlledDestination.Click();
        }

        public bool VerifySelectedControlledDestination(string name, string address, string type)
        {
            try
            {
                var displayedName = verifyControlledDestinationName.Text.Trim();
                var displayedAddress = verifyControlledDestinationAddress.Text.Trim();
                var displayedType = verifyControlledDestinationType.Text.Trim();

                return displayedName.Equals(name) &&
                       displayedAddress.Equals(address) &&
                       displayedType.Equals(type);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void ClickSaveAndContinue()
        {
            btnSaveAndContinue.Click();
        }
    }
}