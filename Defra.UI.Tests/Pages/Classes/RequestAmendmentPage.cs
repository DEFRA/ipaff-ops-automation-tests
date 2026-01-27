using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class RequestAmendmentPage : IRequestAmendmentPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//h1[@class='govuk-heading-xl']"), true);
        private IWebElement chedReference => _driver.FindElement(By.XPath("//span[@class='govuk-caption-xl govuk-!-font-weight-bold govuk-!-margin-bottom-5']"));
        private IWebElement txtStatus => _driver.FindElement(By.Id("Status-Label"));
        private IWebElement txtAmendmentReason => _driver.FindElement(By.Id("request-amendment-additional-details"));
        private IWebElement btnRequestAmendment => _driver.FindElement(By.Id("request-amendment"));
        private IWebElement lnkDoNotRequestAmendment => _driver.FindElement(By.Id("dont-request-amendment"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public RequestAmendmentPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Trim().Contains("Request that the responsible person amends this CHED");
        }

        public void EnterAmendmentReason(string reason)
        {
            txtAmendmentReason.Clear();
            txtAmendmentReason.SendKeys(reason);
        }

        public void ClickRequestAmendmentButton()
        {
            btnRequestAmendment.Click();
        }

        public void ClickDoNotRequestAmendment()
        {
            lnkDoNotRequestAmendment.Click();
        }

        public string GetCHEDReference()
        {
            return chedReference.Text.Trim();
        }

        public string GetStatus()
        {
            return txtStatus.Text.Trim();
        }
    }
}