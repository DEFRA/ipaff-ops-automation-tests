using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class TransportToBorderControlPostPage : ITransportToBorderControlPostPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement optInspectionPremises => _driver.FindElement(By.Id("control-point"));
        private IWebElement optEntryBCP => _driver.FindElement(By.Id("bcp"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public TransportToBorderControlPostPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Description of the goods")
                && primaryTitle.Text.Contains("Transport to the Border Control Post (BCP)");
        }

        public string SelectInspectionPremises(string premises)
        {
            var select = new SelectElement(optInspectionPremises);
            select.SelectByText(premises);
            return select.SelectedOption.GetAttribute("value");
        }

        public void SelectEntryBCP(string entryBCP)
        {
            new SelectElement(optEntryBCP).SelectByText(entryBCP);
        }
    }
}