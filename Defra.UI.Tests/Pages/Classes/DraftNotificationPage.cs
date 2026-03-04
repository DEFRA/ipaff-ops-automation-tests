using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class DraftNotificationPage : IDraftNotificationPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement DraftChedReference => _driver.WaitForElement(By.ClassName("heading-tertiary"), true);
        private IWebElement PageHeading => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IReadOnlyCollection<IWebElement> aMissingOrIncorrectLinks => _driver.FindElements(By.XPath("//a[contains(@id,'error-description')]"));
        private IWebElement h2ErrorSummaryTitle => _driver.FindElement(By.Id("error-summary-title"));
        private IWebElement pErrorSummaryHelpText => _driver.FindElement(By.Id("error-summary-help-text"));
        private IWebElement btnSaveAndContinue => _driver.FindElement(By.Id("button-save-and-continue"));
        private IWebElement lnkCommodityDetails => _driver.FindElement(By.Id("commodity-details-link"));
        #endregion

        public DraftNotificationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return DraftChedReference.Text.Contains("DRAFT") && DraftChedReference.Text.Contains("CHEDPP") && PageHeading.Text.Contains("Review your notification");
        }

        public void ClickSaveAndContinue()
        {
            btnSaveAndContinue.Click();
        }

        public bool AreTitleHelpTextDisplayed()
        {
            return h2ErrorSummaryTitle.Displayed && pErrorSummaryHelpText.Displayed;
        }

        public bool VerifyAllMissingOrErrorLinksExists(IList<string> missingOrErrorLinks)
        {
            var errorLinkTexts = aMissingOrIncorrectLinks
                .Select(e => e.Text.Trim())
                .ToHashSet();

            return missingOrErrorLinks.All(expected =>
            {
                bool exists = errorLinkTexts.Contains(expected);
                Assert.That(exists, $"Expected item '{expected}' was not found in UI list.");
                return exists;
            });
        }

        public string GetDraftNotificationNumber()
        {
            return PageHeading.Text.Trim();
        }

        public void ClickCheckOrUpdateCommodityDetailsLink()
        {
            lnkCommodityDetails.Click();
        }
    }
}
