using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;
using System.Text.RegularExpressions;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterYouHaveSuccessfullySubmittedYourApplicationPage : IExporterYouHaveSuccessfullySubmittedYourApplicationPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[contains(normalize-space(),'You have successfully submitted your application for a certificate of conformity')]"), true);
        private IWebElement lblAphaReference => _driver.WaitForElement(By.Id("applicationFormId"), true);
        #endregion

        public ExporterYouHaveSuccessfullySubmittedYourApplicationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Contains("You have successfully submitted your application");

        public string GetAphaReferenceNumber()
        {
            var raw = lblAphaReference.Text.Trim();
            if (!string.IsNullOrWhiteSpace(raw))
            {
                return raw;
            }

            var bodyText = _driver.WaitForElement(By.TagName("body"), true).Text;
            var match = Regex.Match(bodyText, @"\b\d{4}\s\d{5}\s\d{4}\b");
            return match.Success ? match.Value : string.Empty;
        }
    }
}