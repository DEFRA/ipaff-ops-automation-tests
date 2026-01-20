using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CHEDOverviewPage : ICHEDOverviewPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//h1[@class='govuk-heading-xl ']"), true);
        private IWebElement btnRaiseBorderNotification => _driver.FindElement(By.Id("raise-border-notification"));
        private IWebElement btnCopyAsReplacement => _driver.FindElement(By.Id("replace-certificate"));
        private IWebElement lnkReplacedBy => _driver.FindElement(By.Id("replaced-by"));
        private IWebElement lnkReplacedCertificate => _driver.FindElement(By.Id("replaced-certificate"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public CHEDOverviewPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("CHED overview");
        }

        public void ClickRaiseBorderNotification()
        {
            btnRaiseBorderNotification.Click();
        }

        public void ClickCopyAsReplacement()
        {
            btnCopyAsReplacement.Click();
        }

        public bool VerifyCHEDReference(string type, string chedReference, string replacementChedReference)
        {
            if (type.Equals("original"))
                return primaryTitle.Text.Contains(chedReference);
            else if (type.Equals("replacement"))
                return primaryTitle.Text.Contains(replacementChedReference);
            else
                return false;
        }

        public bool VerifyReplacedByLink(string type, string chedReference, string replacementChedReference)
        {
            if (type.Equals("original"))
                return lnkReplacedCertificate.Text.Contains("Replaced certificate: " + chedReference);
            else if (type.Equals("replacement"))
                return lnkReplacedBy.Text.Contains("Replaced by: " + replacementChedReference);
            else
                return false;
        }

        public void ClickReplacedByLink()
        {
            lnkReplacedBy.Click();
        }
    }
}