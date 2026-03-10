using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class AddAnAgentPage : IAddAnAgentPage
    {
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageHeading => _driver.WaitForElement(By.XPath("//h1[contains(@class,'govuk-heading-xl')]"), true);
        private IWebElement txtAgentCode => _driver.FindElement(By.Id("code"));
        private IWebElement rdoYesIsThisTheAgent => _driver.FindElement(By.XPath(
            "//label[contains(@class,'govuk-label') and (normalize-space(text())='Yes')]"));
        private IWebElement chkDelegation => _driver.FindElement(By.Id("acceptTANDC"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public AddAnAgentPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return pageHeading.Text.Trim().Contains("Add an agent");
        }

        public void EnterAgentCode(string agentCode)
        {
            txtAgentCode.Clear();
            txtAgentCode.SendKeys(agentCode);
        }

        public void SelectYesForIsThisTheAgent()
        {
            rdoYesIsThisTheAgent.Click();
        }

        public void TickDelegationCheckbox()
        {
            if (!chkDelegation.Selected)
                chkDelegation.Click();
        }
    }
}