using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IdmYourDefraAccountPage : IIdmYourDefraAccountPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Your Defra account']"));
        private IWebElement lblFullName => _driver.WaitForElement(By.Id("personal-details__full-name"));
        private IWebElement lblEmail => _driver.WaitForElement(By.Id("personal-details__email"));
        private IWebElement lblPhone => _driver.WaitForElement(By.Id("personal-details__telephone"));
        private IReadOnlyCollection<IWebElement> accountRows => _driver.WaitForElements(By.CssSelector("#main-content table tbody tr[id]"));

        public IdmYourDefraAccountPage(IObjectContainer container)
        {
            _objectContainer = container;
        }
        public bool IsPageLoaded() => pageTitle.Displayed;
        public string GetFullName() => lblFullName.Text.Trim();
        public string GetEmail() => lblEmail.Text.Trim();
        public string GetPhone() => lblPhone.Text.Trim();
        public bool HasExporterAdminAccount(string businessName)
        {
            var matchingRows = accountRows.Where(row => row.FindElements(By.TagName("td")).First().Text.Replace("Account:", "").Trim() == businessName).ToList();
            if (matchingRows.Count != 1)
                return false;
            var account = matchingRows.Single();
            return account.FindElement(By.CssSelector(".idm-table__cell--access-level-content")).Text.Trim() == "Admin"
                && account.FindElement(By.CssSelector(".idm-table__cell--service-name-content")).Text.Trim() == "Apply for a Phytosanitary Certificate"
                && account.FindElement(By.CssSelector(".idm-table__cell--service-role-name-content")).Text.Trim() == "Plant Health Exporter";
        }
        public void CloseAndReturnToExporter(string parentHandle)
        {
            var accountHandle = _driver.CurrentWindowHandle;
            if (accountHandle == parentHandle || !_driver.WindowHandles.Contains(parentHandle))
                throw new InvalidOperationException("The Defra account must be in a separate window with the exporter window still open");
            _driver.Close();
            _driver.SwitchTo().Window(parentHandle);
            if (_driver.WindowHandles.Contains(accountHandle))
                throw new InvalidOperationException("The Defra account window was not closed");
        }
    }
}
