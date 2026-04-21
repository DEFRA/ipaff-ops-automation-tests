using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CheckOrUpdateCommodityDetailsPage: ICheckOrUpdateCommodityDetailsPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement PageHeading => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement lblGrossVolumeOptional => _driver.FindElement(By.XPath("//label[@for='gross-volume']"));
        private IWebElement txtGrossWeight => _driver.FindElement(By.Id("gross-weight"));
        private IWebElement btnSaveAndReview => _driver.FindElement(By.Id("save-and-review-button"));
        private IWebElement drpControlledAdmosphereContainer => _driver.FindElement(By.XPath("//select[@aria-label='Has container']"));
        private IWebElement txtGrossVolume=> _driver.FindElement(By.Id("gross-volume"));
        private IWebElement drpGrossVolumeUnit => _driver.FindElement(By.Id("gross-volume-unit"));

        #endregion

        public CheckOrUpdateCommodityDetailsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return PageHeading.Text.Contains("Check or update commodity details");
        }

        public bool VerifyTotalGrossVolumeIsOptional(string text)
        {
            return lblGrossVolumeOptional.Text.Contains(text);
        }

        public void EnterGrossWeight(string grossWeight)
        {
            txtGrossWeight.SendKeys(grossWeight);
        }

        public void ClickSaveAndReviewButton()
        {
            btnSaveAndReview.Click();
        }

        public string GetControlledAtmosphereContainer()
        {
            return new SelectElement(drpControlledAdmosphereContainer)?.SelectedOption.Text;
        }

        public string GetGrossVolume()=> txtGrossVolume.Text.Trim();

        public string GetGrossVolumeUnit()
        {
            var selected = new SelectElement(drpGrossVolumeUnit).SelectedOption.Text.Trim();

            return selected.Equals("Select Unit", StringComparison.OrdinalIgnoreCase)
                ? string.Empty
                : selected;
        }
    }
}
