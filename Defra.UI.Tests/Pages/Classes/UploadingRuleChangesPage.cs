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

        private IWebElement pageHeading => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Uploading rule changes to the risk engine']"), true);
        private IWebElement checkFileStatusLink => _driver.WaitForElement(By.XPath("//a[normalize-space()='Check file status']"));

        public UploadingRuleChangesPage(IObjectContainer container) => _objectContainer = container;

        public bool IsPageLoaded() => pageHeading.Displayed;
        public void ClickCheckFileStatusLink() => checkFileStatusLink.Click();
    }
}