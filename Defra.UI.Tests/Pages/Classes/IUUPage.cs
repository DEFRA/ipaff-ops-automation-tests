using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;


namespace Defra.UI.Tests.Pages.Classes
{
    public class IUUPage : IIUUPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[@class='govuk-heading-xl']"), true);
        private IWebElement pageSecondaryTitle => _driver.WaitForElement(By.XPath("//span[@class='govuk-caption-xl govuk-!-margin-top-1']"), true);
        private IWebElement rdoYes => _driver.FindElement(By.Id("radio-iuu-selected-yes"));
        private IWebElement rdoComplaint => _driver.FindElement(By.Id("radio-iuu-ok"));
        private IWebElement rdoNotComplaint => _driver.FindElement(By.Id("radio-iuu-not-compliant"));
        private IWebElement rdoNoNeedToInspect => _driver.FindElement(By.Id("radio-iuu-na"));
        private IWebElement rdoNo => _driver.FindElement(By.Id("radio-iuu-selected-no"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public IUUPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return pageTitle.Text.Trim().Contains("IUU")
                && pageSecondaryTitle.Text.Trim().Contains("Illegal, unreported and unregulated fishing");
        }

        public void SelectRecordIUUCheckOption(string option, string subOption)
        {
            if(option.Equals("Yes"))
            {
                rdoYes.Click();
                Thread.Sleep(1000);
                if(subOption.Equals("Compliant"))
                    rdoComplaint.Click();
                else if (subOption.Equals("Not compliant"))
                    rdoNotComplaint.Click();
                if (subOption.Equals("No need to inspect - exempt or not applicable"))
                    rdoNoNeedToInspect.Click();
            }
            else if(option.Equals("No"))
            {
                rdoNo.Click();
            }
        }
    }
}