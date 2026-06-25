using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterReviewYourAnswersPage : IExporterReviewYourAnswersPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Review your answers']"), true);
        private IWebElement btnContinue => _driver.WaitForElement(By.Id("Button-Continue"));
        #endregion

        public ExporterReviewYourAnswersPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Review your answers");
        public void ClickContinueButton() => btnContinue.Click();
    }
}