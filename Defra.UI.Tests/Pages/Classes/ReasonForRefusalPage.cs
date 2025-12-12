using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ReasonForRefusalPage : IReasonForRefusalPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//h1[@class='govuk-heading-xl govuk-!-margin-bottom-6 govuk-!-margin-top-6 ']"), true);
        private IWebElement getReasonForRefusalCheckBoxes(string reason) => _driver.FindElement(By.XPath($"//label[normalize-space()='{reason}']"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ReasonForRefusalPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Reason for refusal");
        }

        public void SelectReasonForRefusal(params string[] reasons)
        {
            foreach (var reason in reasons)
            {
                var reasonCheckBox = getReasonForRefusalCheckBoxes(reason);
                reasonCheckBox.Click();
            }
        }
    }
}