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
        private IWebElement confirmationTitle => _driver.WaitForElement(By.Id("confirmation-title"), true);
        private IWebElement lblAphaReference => _driver.WaitForElement(By.Id("applicationFormId"), true);
        private IWebElement btnViewYourApplications => _driver.WaitForElement(By.Id("Link-ViewYourApplications"));
        #endregion

        public ExporterYouHaveSuccessfullySubmittedYourApplicationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Contains("You have successfully submitted your application");

        public bool IsPhytosanitaryConfirmationPageLoaded() => confirmationTitle.Text.Trim().Equals("Application complete") && lblAphaReference.Displayed;

        public bool IsSeedSamplingConfirmationPageLoaded() => confirmationTitle.Text.Trim().Equals("You have successfully submitted your seed sampling application") && lblAphaReference.Displayed;

        public string GetAphaReferenceNumber()
        {
            var raw = lblAphaReference.Text.Trim();
            if (!string.IsNullOrWhiteSpace(raw))
            {
                Console.WriteLine("[APPLICATION SUBMITTED] APHA Reference: " + raw);
                return raw.Replace(" ", "");
            }

            var bodyText = _driver.WaitForElement(By.TagName("body"), true).Text;
            var match = Regex.Match(bodyText, @"\b\d{4}\s\d{5}\s\d{4}\b");
            var reference = match.Success ? match.Value.Replace(" ", "") : string.Empty;
            Console.WriteLine("[APPLICATION SUBMITTED] APHA Reference: " + reference);
            return reference;
        }

        public void ClickViewYourApplicationsButton() => btnViewYourApplications.Click();
    }
}