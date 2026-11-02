using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IdmManageAccountPage : IIdmManageAccountPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Manage account']"));
        private IWebElement lnkDefraAccount => _driver.WaitForElement(By.Id("link-defra-account"));
        private IWebElement lnkBack => _driver.WaitForElement(By.Id("link-back"));

        public IdmManageAccountPage(IObjectContainer container)
        {
            _objectContainer = container;
        }
        public bool IsPageLoaded() => pageTitle.Displayed;
        public string OpenDefraAccountInNewWindow()
        {
            var parent = _driver.CurrentWindowHandle;
            var existingHandles = _driver.WindowHandles.ToHashSet();
            lnkDefraAccount.Click();
            var newHandle = _driver.WaitForElementCondition(d =>
            {
                var added = d.WindowHandles.Where(h => !existingHandles.Contains(h)).ToList();
                return added.Count == 1 ? added[0] : null;
            });
            _driver.SwitchTo().Window(newHandle);
            return parent;
        }
        public void ClickBackLink() => lnkBack.Click();
    }
}
