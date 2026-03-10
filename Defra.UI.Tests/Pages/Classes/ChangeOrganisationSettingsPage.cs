using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ChangeOrganisationSettingsPage : IChangeOrganisationSettingsPage
    {
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageHeading => _driver.WaitForElement(By.XPath("//h1[contains(@class,'govuk-heading-xl')]"), true);
        private IWebElement lblInstructionText => _driver.FindElement(By.XPath("//p[contains(@class,'govuk-body') and contains(text(),'Below are the available options')]"));
        private IWebElement lblSelectAllThatApply => _driver.FindElement(By.XPath("//div[contains(@class,'govuk-hint') and contains(text(),'Select all that apply')]"));
        private IWebElement chkAuthoriseAgent => _driver.FindElement(By.Id("business"));
        private IWebElement lblAuthoriseAgent => _driver.FindElement(By.XPath("//label[@for='business']"));
        private IWebElement chkActAsAgent => _driver.FindElement(By.Id("agent"));
        private IWebElement lblActAsAgent => _driver.FindElement(By.XPath("//label[@for='agent']"));
        private IWebElement agentDeclarationInset => _driver.FindElement(By.Id("prompt-for-agent"));
        private IWebElement chkConfirmation => _driver.FindElement(By.Id("agentAcceptance"));
        private IWebElement lblConfirmation => _driver.FindElement(By.XPath("//label[@for='agentAcceptance']"));
        private IWebElement warningText => _driver.FindElement(By.XPath("//strong[contains(@class,'govuk-warning-text__text')]"));
        private IWebElement btnSave => _driver.FindElement(By.XPath("//button[@type='submit' and contains(@class,'govuk-button')]"));
        private IWebElement confirmationPanel => _driver.WaitForElement(By.XPath("//div[contains(@class,'govuk-panel--confirmation')]"), true);
        private IWebElement confirmationPanelTitle => _driver.FindElement(By.XPath("//div[contains(@class,'govuk-panel--confirmation')]//h1[contains(@class,'govuk-panel__title')]"));
        private IWebElement btnContinue => _driver.FindElement(By.XPath("//a[contains(@class,'govuk-button') and normalize-space(text())='Continue']"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ChangeOrganisationSettingsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return pageHeading.Text.Trim().Contains("Change organisation settings");
        }

        public bool IsConfirmationPageLoaded(string expectedMessage)
        {
            return IsPageLoaded()
                && confirmationPanel.Displayed
                && confirmationPanelTitle.Text.Trim().Contains(expectedMessage);
        }

        public bool IsInstructionTextDisplayed()
        {
            return lblInstructionText.Displayed
                && lblInstructionText.Text.Contains("Below are the available options you may change for this organisation. Select the relevant options and click save.");
        }

        public bool IsSelectAllThatApplyHintDisplayed()
        {
            return lblSelectAllThatApply.Displayed
                && lblSelectAllThatApply.Text.Trim().Contains("Select all that apply.");
        }

        public bool IsAuthoriseAgentCheckboxDisplayed()
        {
            return lblAuthoriseAgent.Displayed
                && lblAuthoriseAgent.Text.Trim().Contains("I want to authorise an agent to act for my business");
        }

        public bool IsActAsAgentCheckboxDisplayed()
        {
            return lblActAsAgent.Displayed
                && lblActAsAgent.Text.Trim().Contains("I am an agent who wants authority to act on behalf of other businesses");
        }

        public bool IsAgentDeclarationTextDisplayed()
        {
            var text = agentDeclarationInset.Text;
            return agentDeclarationInset.Displayed
                && text.Contains("I am an agent who wants authority to act on behalf of other businesses")
                && text.Contains("I understand that by using this service as an agent")
                && text.Contains("I can view and complete export applications and import notifications for my client")
                && text.Contains("I may incur charges for my client");
        }

        public bool IsConfirmationCheckboxDisplayed()
        {
            return lblConfirmation.Displayed
                && lblConfirmation.Text.Trim().Contains("I confirm that I have read and accept the above statement/s.");
        }

        public bool IsWarningMessageDisplayed()
        {
            return warningText.Displayed
                && warningText.Text.Contains("If you are turning an option off, any delegations associated with that option will be removed and permissions revoked.");
        }

        public bool IsSaveButtonDisplayed()
        {
            return btnSave.Displayed;
        }

        public void TickAuthoriseAgentCheckbox()
        {
            if (!chkAuthoriseAgent.Selected)
                chkAuthoriseAgent.Click();
        }

        public void UntickActAsAgentCheckbox()
        {
            if (chkActAsAgent.Selected)
                chkActAsAgent.Click();
        }

        public void TickConfirmationCheckbox()
        {
            if (!chkConfirmation.Selected)
                chkConfirmation.Click();
        }

        public void ClickSave()
        {
            btnSave.Click();
        }

        public void ClickContinue()
        {
            btnContinue.Click();
        }
    }
}