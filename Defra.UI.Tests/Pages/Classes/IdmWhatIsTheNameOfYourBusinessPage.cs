using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IdmWhatIsTheNameOfYourBusinessPage : IIdmWhatIsTheNameOfYourBusinessPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='What is the name of your business?']"));
        private IWebElement txtBusinessName => _driver.WaitForElement(By.Id("companyName"));
        private IWebElement btnContinue => _driver.WaitForElementClickable(By.Id("button-continue"));

        public IdmWhatIsTheNameOfYourBusinessPage(IObjectContainer container)
        {
            _objectContainer = container;
        }
        public bool IsPageLoaded() => pageTitle.Displayed;
        public void EnterBusinessNameAndContinue(string name)
        {
            txtBusinessName.SendKeys(name);
            btnContinue.Click();
        }
    }
}
