using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ManageYourAuthorisationsPage : IManageYourAuthorisationsPage
    {
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageHeading => _driver.WaitForElement(By.XPath("//h1[contains(@class,'govuk-heading-xl')]"), true);
        private By GetBusinessNameHeaderBy(string businessName) => By.XPath($"//h2[normalize-space(text())='{businessName}']");
        private IWebElement GetBusinessNameHeader(string businessName) => _driver.FindElement(GetBusinessNameHeaderBy(businessName));
        private IWebElement changeSettingsInsetText => _driver.FindElement(By.XPath("//div[contains(@class,'govuk-inset-text') and contains(.,'To change settings related to this organisation')]"));
        private IWebElement lnkChangeSettings => _driver.FindElement(By.XPath("//a[contains(@href,'Change-Settings')]"));
        private IWebElement hdrBusinessesAuthorisedToRepresent => _driver.FindElement(By.XPath("//h2[contains(text(),'Businesses you are authorised to represent')]"));
        private IWebElement agentCodeInset => _driver.FindElement(By.XPath("//div[contains(@class,'govuk-inset-text') and contains(.,'Your agent code is')]"));
        private IWebElement autoAcceptDelegationTableHeader => _driver.FindElement(By.XPath("//th[contains(normalize-space(text()),'Automatically accept delegation requests from Importers/Exporters?')]"));
        private IWebElement autoAcceptToggleYes => _driver.FindElement(By.XPath("//span[contains(@class,'permission-tag-on') and not(contains(@style,'display:none'))]"));
        private IWebElement hdrCompanies => _driver.FindElement(By.XPath("//h3[normalize-space(text())='Companies']"));
        private IWebElement lblNoPermissions => _driver.FindElement(By.XPath("//p[contains(text(),'You have not been assigned any permissions by importers/exporters')]"));
        private IReadOnlyCollection<IWebElement> organisationTabs => _driver.FindElements(By.CssSelector("li.organisation-selection"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ManageYourAuthorisationsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return pageHeading.Text.Trim().Contains("Manage your authorisations");
        }

        public bool IsBusinessNameDisplayedAsHeader(string businessName)
        {
            return _driver.IsElementDisplayed(GetBusinessNameHeaderBy(businessName));
        }

        public bool IsChangeSettingsLinkDisplayed()
        {
            var insetText = changeSettingsInsetText.Text;
            return insetText.Contains("To change settings related to this organisation,")
                && lnkChangeSettings.Displayed
                && lnkChangeSettings.Text.Contains("click here");
        }

        public void ClickChangeSettingsLink()
        {
            lnkChangeSettings.Click();
        }

        public bool IsBusinessesYouAreAuthorisedToRepresentHeaderDisplayed()
        {
            return hdrBusinessesAuthorisedToRepresent.Displayed;
        }

        public bool IsAgentCodeDisplayed(string agentCode)
        {
            var insetText = agentCodeInset.Text;
            return insetText.Contains("Your agent code is")
                && insetText.Contains(agentCode)
                && insetText.Contains("Give this code to any business that wants to authorise you to act on their behalf.");
        }

        public bool IsAutomaticallyAcceptDelegationToggledYes()
        {
            return autoAcceptDelegationTableHeader.Displayed
                && autoAcceptToggleYes.Displayed;
        }

        public bool IsCompaniesWithNoPermissionsDisplayed()
        {
            return hdrCompanies.Displayed
                && lblNoPermissions.Displayed;
        }

        public bool AreCompaniesListed(string trader1BusinessName, string trader2BusinessName)
        {
            var tabs = organisationTabs.Select(t => t.Text.Trim()).ToList();
            return tabs.Any(t => t.Contains(trader1BusinessName))
                && tabs.Any(t => t.Contains(trader2BusinessName));
        }
    }
}