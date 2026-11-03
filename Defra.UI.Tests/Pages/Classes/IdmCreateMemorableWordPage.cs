using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IdmCreateMemorableWordPage : IIdmCreateMemorableWordPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Create a memorable word']"));
        private IWebElement txtWord => _driver.WaitForElement(By.Id("memorableWord"));
        private IWebElement txtHint => _driver.WaitForElement(By.Id("securityHint"));
        private IWebElement btnContinue => _driver.WaitForElementClickable(By.Id("button-continue"));
        #endregion

        public IdmCreateMemorableWordPage(IObjectContainer container)
        {
            _objectContainer = container;
        }
        public bool IsPageLoaded() => pageTitle.Displayed;
        public void EnterWordAndHintAndContinue(string word, string hint)
        {
            txtWord.SendKeys(word);
            txtHint.SendKeys(hint);
            btnContinue.Click();
        }
    }
}
