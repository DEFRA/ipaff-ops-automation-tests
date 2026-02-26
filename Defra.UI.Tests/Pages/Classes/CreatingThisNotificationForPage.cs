using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CreatingThisNotificationForPage: ICreatingThisNotificationForPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement PageHeading => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement rdoAgent => _driver.FindElement(By.XPath("//label[@for='for-own-organisation']"));
        private IWebElement rdoADifferentOrganisation => _driver.FindElement(By.XPath("//label[@for='for-own-organisation']"));

        private IWebElement btnSaveAndReview => _driver.FindElement(By.Id("button-save-and-review"));
        #endregion

        public CreatingThisNotificationForPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return PageHeading.Text.Contains("Who are you creating this notification for?");
        }

        public void SelectNotificationForOption(string option)
        {
            if(option.ToUpper().Equals("AGENT1"))
            {
                rdoAgent.Click();
            }
            else if(option.ToUpper().Equals("A DIFFERENT ORGANISATION"))
            {
                rdoADifferentOrganisation.Click();
            }
        }

        public void ClickSaveAndReviewButton()
        {
            btnSaveAndReview.Click();
        }
    }
}
