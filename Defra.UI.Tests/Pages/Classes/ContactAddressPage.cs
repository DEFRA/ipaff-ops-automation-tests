using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;


namespace Defra.UI.Tests.Pages.Classes
{
    public class ContactAddressPage : IContactAddressPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement firstContactAddressLabel => _driver.FindElement(By.XPath("//label[contains(@for, 'branch-address')]"));
        private IWebElement rdoContactAddress => _driver.FindElement(By.XPath("//label[contains(@for, 'branch-address')]/preceding-sibling::input[contains(@id,'branch-address')]"));
        private IWebElement draftChedRefNumber => _driver.FindElement(By.Id("reference-number"));

        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ContactAddressPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Complete notification")
                && primaryTitle.Text.Contains("Contact address for consignment");
        }

        public bool IsContactAddressForConsignmentPageLoaded()
        {
            return primaryTitle.Text.Contains("Contact address for consignment")
                && secondaryTitle.Text.Contains("Contacts");
        }

        public bool IsPageLoadedWithoutSecondaryTitle()
        {
            return primaryTitle.Text.Contains("Contact address for consignment");
        }

        public string GetSelectedContactAddress()
        {
            var fullAddress = firstContactAddressLabel.Text.Trim();
            var lines = fullAddress.Split(new[] { "\r\n", "\n", "," }, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(l => l.Trim())
                                    .ToList();

            // This excludes the country which is not displayed on the review page and also it works for all address irrespective of number of lines in address
            lines.RemoveAt(lines.Count - 1); 
            return string.Join("\n", lines).Trim();
        }

        public void SelectContactAddressRadio() => rdoContactAddress.Click();

        public bool IsContactAddressRadioButtonSelected() => rdoContactAddress.Selected;

        public string GetDraftCHEDRefNumber => draftChedRefNumber?.Text.Trim() ?? string.Empty;
    }
}