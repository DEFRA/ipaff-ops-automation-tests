using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CheckFileProcessingStatusPage : ICheckFileProcessingStatusPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        private IWebElement pageHeading => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Check your file processing status']"), true);
        private IWebElement viewStatusLink => _driver.WaitForElement(By.XPath("//a[normalize-space()='view the processing status of your file here']"));

        public CheckFileProcessingStatusPage(IObjectContainer container) => _objectContainer = container;

        public bool IsPageLoaded() => pageHeading.Displayed;
        public void ClickViewProcessingStatusLink() => viewStatusLink.Click();
    }
}