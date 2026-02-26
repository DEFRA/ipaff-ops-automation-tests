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
        private IWebElement PageHeading => _driver.WaitForElement(By.ClassName("heading-tertiary"), true);
        private IReadOnlyCollection<IWebElement> aMissingOrIncorrectLinks => _driver.FindElements(By.XPath("//a[contains(@id,'error-description')]"));
        private IWebElement h2ErrorSummaryTitle => _driver.FindElement(By.Id("error-summary-title"));
        private IWebElement pErrorSummaryHelpText => _driver.FindElement(By.Id("error-summary-help-text"));
        private IWebElement btnSaveAndContinue => _driver.FindElement(By.Id("button-save-and-continue"));
        #endregion

        public DraftNotificationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            var text = PageHeading.Text.Trim();
            return PageHeading.Text.Contains("DRAFT") && PageHeading.Text.Contains("CHEDPP");
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

        public void ClickEachLinkAndEnterMissingInformation()
        {
            foreach (var link in aMissingOrIncorrectLinks)
            {
                var linkText = link.Text?.Trim()?.ToUpper();

                link.Click();

                switch (linkText)
                {
                    case "ADD THE ESTIMATED ARRIVAL DATE AT BCP":
                        break;
                    case "ADD THE ESTIMATED ARRIVAL TIME AT BCP":
                        break;
                    case "ENTER MISSING COMMODITY DETAILS":
                        break;
                    case "ADD THE TOTAL GROSS WEIGHT":
                        break;
                    case "CHECK YOUR DETAILS ON THE 'CONTACT DETAILS' PAGE AND SAVE THEM TO CONTINUE":
                        break;
                    case "SELECT IF USING THE GOODS VEHICLE MOVEMENT SERVICE (GVMS)":
                        break;
                    case "ADD DOCUMENT DETAILS":
                        break;
                    case "ADD A DELIVERY ADDRESS":
                        break;
                    case "ADD THE ENTRY BORDER CONTROL POST":
                        break;
                    case "SELECT IF USING THE COMMON TRANSIT CONVENTION (CTC)":
                        break;
                }
            }
        }

        public string GetDraftNotificationNumber()
        {
            return PageHeading.Text.Trim();
        }
    }
}
