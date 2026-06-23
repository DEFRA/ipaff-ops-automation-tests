using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class UploadingRuleChangesPage : IUploadingRuleChangesPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageHeading => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Uploading rule changes to the risk engine']"), true);
        private IWebElement checkFileStatusLink => _driver.WaitForElement(By.XPath("//a[normalize-space()='Check file status']"));
        #endregion

        public UploadingRuleChangesPage(IObjectContainer container) => _objectContainer = container;

        public bool IsPageLoaded() => pageHeading.Text.Trim().Equals("Uploading rule changes to the risk engine");
        public void ClickCheckFileStatusLink() => checkFileStatusLink.Click();
    }
}