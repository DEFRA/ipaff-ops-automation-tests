using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using SeleniumExtras.WaitHelpers;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System.Collections.ObjectModel;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using Faker;

namespace Defra.UI.Tests.Pages.Classes
{
    public class TransporterPage : ITransporterPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement lnkAddTransporter => _driver.WaitForElement(By.Id("add-transporter-from-bip"));
        private IWebElement selectedTransporter => _driver.WaitForElement(By.XPath("//td[contains(@headers, 'transporter-company-name-address-country')]"));
        private IWebElement btnSaveAndContinue => _driver.WaitForElement(By.Id("save-and-return-button-desktop"));
        private IWebElement verifyTransporterNameAddressCountry => _driver.FindElement(By.XPath("//td[@headers='transporter-company-name-address-country']"));
        private IWebElement verifyTransporterApprovalNumber => _driver.FindElement(By.XPath("//td[@headers='transporter-number']"));
        private IWebElement verifyTransporterType => _driver.FindElement(By.XPath("//td[@headers='transporter-type']"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public TransporterPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Transport")
                && primaryTitle.Text.Contains("Transporter");
        }

        public void ClickAddTransporter()
        {
            lnkAddTransporter.Click();
        }

        public bool VerifySelectedTransporter()
        {
            return selectedTransporter.Text.Contains("VOGNMAND HANS SKOV CHRISTENSEN A/S");
        }

        public void ClickSaveAndContinue()
        {
            btnSaveAndContinue.Click();
        }

        public bool VerifySelectedTransporter(string name, string address, string country, string approvalNumber, string type)
        {
            try
            {
                Console.WriteLine($"[VERIFY PAGE] Expected - Name: '{name}', Address: '{address}', Country: '{country}', Approval: '{approvalNumber}', Type: '{type}'");

                var nameAddressCountryCell = verifyTransporterNameAddressCountry;
                var paragraphs = nameAddressCountryCell.FindElements(By.TagName("p"));

                // Extract text from each paragraph
                var displayedName = paragraphs.Count > 0 ? paragraphs[0].Text.Trim() : "";
                var displayedAddress = paragraphs.Count > 1 ? paragraphs[1].Text.Trim() : "";
                var displayedCountry = paragraphs.Count > 2 ? paragraphs[2].Text.Trim() : "";

                var displayedApprovalNumber = verifyTransporterApprovalNumber.Text.Trim();
                var displayedType = verifyTransporterType.Text.Trim();

                Console.WriteLine($"[VERIFY PAGE] Displayed - Name: '{displayedName}', Address: '{displayedAddress}', Country: '{displayedCountry}', Approval: '{displayedApprovalNumber}', Type: '{displayedType}'");

                // Log individual comparisons
                Console.WriteLine($"[VERIFY PAGE] Name Match: {displayedName.Equals(name)}");
                Console.WriteLine($"[VERIFY PAGE] Address Match: {displayedAddress.Equals(address)}");
                Console.WriteLine($"[VERIFY PAGE] Country Match: {displayedCountry.Equals(country)}");
                Console.WriteLine($"[VERIFY PAGE] Approval Number Match: {displayedApprovalNumber.Equals(approvalNumber)}");
                Console.WriteLine($"[VERIFY PAGE] Type Match: {displayedType.Equals(type)}");

                return displayedName.Equals(name) &&
                       displayedAddress.Equals(address) &&
                       displayedCountry.Equals(country) &&
                       displayedApprovalNumber.Equals(approvalNumber) &&
                       displayedType.Equals(type);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VERIFY PAGE] Exception during verification: {ex.Message}");
                return false;
            }
        }
    }
}