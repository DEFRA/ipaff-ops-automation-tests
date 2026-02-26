using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CheckOrUpdateCommodityDetails: ICheckOrUpdateCommodityDetails
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement PageHeading => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement lblGrossVolumeOptional => _driver.FindElement(By.XPath("//label[@for='gross-volume']"));
        private IWebElement txtGrossWeight => _driver.FindElement(By.Id("gross-weight"));
        private IWebElement btnSaveAndReview => _driver.FindElement(By.Id("save-and-review-button"));

        #endregion

        public CheckOrUpdateCommodityDetails(IObjectContainer container)
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
    }
}
