using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterYourApplicationForSeedSamplingPage : IExporterYourApplicationForSeedSamplingPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.Id("main-heading"), true);
        private IWebElement lnkSeedsToBeSampled => _driver.WaitForElement(By.Id("section1-1-link"));
        private IWebElement lnkInspectionDetails => _driver.WaitForElement(By.Id("section1-2-link"));
        #endregion

        public ExporterYourApplicationForSeedSamplingPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Your application for seed sampling");

        public void ClickSeedsToBeSampledLink() => lnkSeedsToBeSampled.Click();

        public void ClickInspectionDetailsLink() => lnkInspectionDetails.Click();
    }
}
