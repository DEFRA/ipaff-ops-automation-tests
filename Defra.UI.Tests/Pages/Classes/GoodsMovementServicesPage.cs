using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;


namespace Defra.UI.Tests.Pages.Classes
{
    public class GoodsMovementServicesPage : IGoodsMovementServicesPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement rdoYesMRNNow => _driver.FindElement(By.XPath("//*[@id='ctc-question-yes']/following-sibling::label"));
        private IWebElement rdoYesMRNLater => _driver.FindElement(By.XPath("//*[@id='ctc-question-yes-add-later']/following-sibling::label"));
        private IWebElement rdoNo => _driver.FindElement(By.XPath("//*[@id='ctc-question-no']/following-sibling::label"));
        private IWebElement gvmsNoRadio => _driver.FindElement(By.XPath("//*[@id='gvms-question']/following-sibling::label"));
        private IWebElement gvmsYesRadio => _driver.FindElement(By.XPath("//*[@id='ctc-question-no']/following-sibling::label"));
        private List<IWebElement> gvmsRadioGroup => _driver.WaitForElements(By.XPath("//input[contains(@id,'gvms-question')]/following-sibling::label")).ToList();
        private IWebElement rdoGVMSYes => _driver.FindElement(By.XPath("//*[@id='gvms-question']/following-sibling::label"));
        private IWebElement rdoGVMSNo => _driver.FindElement(By.XPath("//*[@id='gvms-question-2']/following-sibling::label"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public GoodsMovementServicesPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Transport")
                && primaryTitle.Text.Contains("Goods movement services");
        }

        public void CTCToMoveGoods(string option)
        {
            if (rdoYesMRNNow.Text.Trim().Contains(option))
                rdoYesMRNNow.Click();
            else if (rdoYesMRNLater.Text.Trim().Contains(option))
                rdoYesMRNLater.Click();
            else if (rdoNo.Text.Trim().Contains(option))
                rdoNo.Click();
        }

        public void GVMSToMoveGoods(string option)
        {
            if (rdoGVMSYes.Text.Trim().Contains(option))
                rdoGVMSYes.Click();
            else if (rdoGVMSNo.Text.Trim().Contains(option))
                rdoGVMSNo.Click();
        }
    }
}