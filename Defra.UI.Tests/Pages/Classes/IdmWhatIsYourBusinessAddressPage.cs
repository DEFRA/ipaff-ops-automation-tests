using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IdmWhatIsYourBusinessAddressPage : IIdmWhatIsYourBusinessAddressPage
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        private By pageTitleBy => By.XPath("//h1[contains(normalize-space(),'What is your business address') or contains(normalize-space(),'What’s the business’s address')]");
        private IWebElement txtBuildingNumber => _driver.WaitForElement(By.Id("buildingNumber"));
        private IWebElement txtStreet => _driver.WaitForElement(By.Id("street"));
        private IWebElement txtTown => _driver.WaitForElement(By.Id("town"));
        private IWebElement txtPostcode => _driver.WaitForElement(By.Id("postcode"));
        private IWebElement btnContinue => _driver.WaitForElementClickable(By.Id("button-continue"));

        public IdmWhatIsYourBusinessAddressPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsDisplayed() => _driver.FindElements(pageTitleBy).Any(element => element.Displayed);

        public void EnterAddressAndContinue(string buildingNumber, string street, string town, string postcode)
        {
            txtBuildingNumber.Clear();
            txtBuildingNumber.SendKeys(buildingNumber);
            txtStreet.Clear();
            txtStreet.SendKeys(street);
            txtTown.Clear();
            txtTown.SendKeys(town);
            txtPostcode.Clear();
            txtPostcode.SendKeys(postcode);
            btnContinue.Click();
        }
    }
}
