using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CatchCertificatesPage : ICatchCertificatesPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement questionHeading => _driver.WaitForElement(By.XPath("//h1[contains(@class, 'govuk-heading-l')]"), true);
        private IWebElement rdoAddCertificateYes => _driver.FindElement(By.XPath("//*[@id='catch-certificate-needed']/following-sibling::label"));
        private IWebElement rdoAddCertificateNo => _driver.FindElement(By.XPath("//*[@id='catch-certificate-needed-2']/following-sibling::label"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public CatchCertificatesPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Documents")
                && primaryTitle.Text.Contains("Catch certificates");
        }

        public void SelectAddCatchCertificate(string option)
        {
            if (option.Equals(rdoAddCertificateYes.Text))
                rdoAddCertificateYes.Click();
            else if (rdoAddCertificateNo.Text.Trim().Contains(option))
                rdoAddCertificateNo.Click();
        }

        public bool VerifyQuestionDisplayed(string questionText)
        {
            return questionHeading.Text.Trim().Equals(questionText, StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifyRadioButtonsDisplayed(string yesText, string noText)
        {
            var yesLabelText = rdoAddCertificateYes.Text.Trim();
            var noLabelText = rdoAddCertificateNo.Text.Trim();

            return yesLabelText.Equals(yesText, StringComparison.OrdinalIgnoreCase) &&
                   noLabelText.Equals(noText, StringComparison.OrdinalIgnoreCase);
        }
    }
}