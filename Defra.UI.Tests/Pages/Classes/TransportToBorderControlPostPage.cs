using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.Script;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            Console.WriteLine("Primary " + primaryTitle.Text);
            Console.WriteLine("Secondary " + secondaryTitle.Text);

            return secondaryTitle.Text.Contains("Description of the goods")
                && primaryTitle.Text.Contains("Transport to the Border Control Post (BCP)");
        }

        public void SelectInspectionPremises(string premises)
        {
            new SelectElement(optInspectionPremises).SelectByText(premises);
        }

        public void SelectEntryBCP(string entryBCP)
        {
            new SelectElement(optEntryBCP).SelectByText(entryBCP);
        }
    }
}