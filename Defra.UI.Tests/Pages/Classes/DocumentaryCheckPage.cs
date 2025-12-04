using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class DocumentaryCheckPage : IDocumentaryCheckPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement rdoSatisfactory => _driver.FindElement(By.Id("document-check-result"));
        private IWebElement rdoSatisfactoryOfficialIntervention => _driver.FindElement(By.Id("conditional-document-check-8"));
        private IWebElement rdoNotSatisfactory => _driver.FindElement(By.Id("document-check-result-2"));
        private IWebElement rdoNotDone => _driver.FindElement(By.Id("document-check-result-7"));
        private IWebElement btnSaveAndContinue => _driver.FindElement(By.Id("button-save-and-continue"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public DocumentaryCheckPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Documentary check");
        }

        public void SelectDocumentaryCheckDecision(string decision)
        {
            if(decision.Equals("Satisfactory"))
                rdoSatisfactory.Click();
            else if(decision.Equals("Satisfactory following official intervention"))
                rdoSatisfactoryOfficialIntervention.Click();
            else if (decision.Equals("Not satisfactory"))
                rdoNotSatisfactory.Click();
            else if (decision.Equals("Not done"))
                rdoNotDone.Click();
        }

        public void ClickSaveAndContinue()
        {
            btnSaveAndContinue.Click();
        }
    }
}