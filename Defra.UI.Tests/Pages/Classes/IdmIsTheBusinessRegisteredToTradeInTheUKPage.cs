using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IdmIsTheBusinessRegisteredToTradeInTheUKPage : IIdmIsTheBusinessRegisteredToTradeInTheUKPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Is the business registered to trade in the UK?']"));
        private IWebElement lblOption(string option) => _driver.WaitForElement(By.XPath($"//label[normalize-space()='{option}']"));
        private IWebElement btnContinue => _driver.WaitForElementClickable(By.Id("button-continue"));

        public IdmIsTheBusinessRegisteredToTradeInTheUKPage(IObjectContainer container)
        {
            _objectContainer = container;
        }
        public bool IsPageLoaded() => pageTitle.Displayed;
        public void SelectOptionAndContinue(string option)
        {
            _driver.WaitForElement(By.Id(lblOption(option).GetAttribute("for"))).Click();
            btnContinue.Click();
        }
    }
}
