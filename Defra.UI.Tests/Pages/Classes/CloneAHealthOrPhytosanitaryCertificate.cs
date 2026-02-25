using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;
using System.Collections.ObjectModel;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CloneAHealthOrPhytosanitaryCertificate : ICloneAHealthOrPhytosanitaryCertificate
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement PageHeading => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private ReadOnlyCollection<IWebElement> lblContentLIst => _driver.FindElements(By.XPath("//p[@class='govuk-body']"));
        private IWebElement h2Title => _driver.FindElement(By.XPath("//h2[@class='govuk-heading-l']"));
        private ReadOnlyCollection<IWebElement> liCertificateDetailsList => _driver.FindElements(By.XPath("//ul[contains(@class,'govuk-list--bullet')]/li"));
        private IWebElement legImportingOptionQuestion => _driver.FindElement(By.XPath("//legend[contains(@class,'govuk-fieldset__legend')]"));
        private IReadOnlyCollection<IWebElement> rdoImportOptions => _driver.FindElements(By.XPath("//div[@class='govuk-radios__item']"));
        private IWebElement setImportingOption(int index) => _driver.FindElement(By.XPath($"//label[@for='cert-type-{index}']"));
        private IReadOnlyCollection<IWebElement> lblFieldTitleList => _driver.FindElements(By.XPath("//label[@class='govuk-label govuk-label--m']| //legend[contains(@class,'govuk-fieldset__legend')]"));

        #endregion

        public CloneAHealthOrPhytosanitaryCertificate(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return PageHeading.Text.Contains("Clone a health or phytosanitary certificate");
        }

        public void SelectImportingOption(string importOption)
        {
            if (importOption.ToUpper().Equals("LIVE ANIMALS"))
            {
                setImportingOption(1).Click();
            }
            else if (importOption.ToUpper().Equals("PRODUCTS OF ANIMAL ORIGIN, GERMINAL PRODUCTS OR ANIMAL BY-PRODUCTS"))
            {
                setImportingOption(2).Click();
            }
            else if (importOption.ToUpper().Equals("PLANTS, PLANT PRODUCTS AND OTHER OBJECTS"))
            {
                setImportingOption(3).Click();
            }
        }
    }
}
