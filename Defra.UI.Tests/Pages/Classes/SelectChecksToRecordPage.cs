using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class SelectChecksToRecordPage : ISelectChecksToRecordPage
    {
        private readonly IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle =>
            _driver.WaitForElement(By.Id("page-primary-title"));

        private IWebElement checksStillToDoContainer =>
            _driver.FindElement(By.CssSelector(".govuk-inset-text"));

        private IWebElement chkDocumentary =>
            _driver.FindElement(By.Id("documentary"));

        private IWebElement chkIdentity =>
            _driver.FindElement(By.Id("identity"));

        private IWebElement chkPhysical =>
            _driver.FindElement(By.Id("physical"));

        private IWebElement btnContinue =>
            _driver.FindElement(By.Id("button-save-and-continue"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public SelectChecksToRecordPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Trim()
                .Equals("Select which checks to record", StringComparison.OrdinalIgnoreCase);
        }

        public bool IsCheckStillToDo(string checkType)
        {
            // The inset text contains paragraphs like "<strong>448</strong> documentary checks"
            // Match on the paragraph text containing the check type (e.g. "documentary checks")
            var paragraphs = checksStillToDoContainer
                .FindElements(By.TagName("p"));

            return paragraphs.Any(p =>
                p.Text.Contains(checkType, StringComparison.OrdinalIgnoreCase));
        }

        public void TickAllCheckboxes()
        {
            TickIfUnchecked(chkDocumentary);
            TickIfUnchecked(chkIdentity);
            TickIfUnchecked(chkPhysical);
        }

        public void ClickContinue()
        {
            btnContinue.Click();
        }

        private static void TickIfUnchecked(IWebElement checkbox)
        {
            if (!checkbox.Selected)
                checkbox.Click();
        }
    }
}