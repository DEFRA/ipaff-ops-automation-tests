using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterYourApplicationPage : IExporterYourApplicationPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Your application']"), true);
        private IWebElement lnkWhatsInYourConsignment => _driver.WaitForElement(By.XPath("//a[normalize-space()=\"What's in your consignment?\"]"));
        private IWebElement lnkInspectionDetails => _driver.WaitForElement(By.XPath("//a[normalize-space()='What are the inspection details?']"));
        private IWebElement lnkTransport => _driver.WaitForElement(By.XPath("//a[normalize-space()='How will this consignment be transported?']"));
        private IWebElement lnkPackerDetails => _driver.WaitForElement(By.XPath("//a[normalize-space()='What are the packer details?']"));
        private IWebElement lnkCheckAnswers => _driver.WaitForElement(By.XPath("//a[normalize-space()='Check your answers and submit your application']"));
        #endregion

        public ExporterYourApplicationPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Your application");
        public void ClickWhatsInYourConsignmentLink() => lnkWhatsInYourConsignment.Click();
        public void ClickWhatAreTheInspectionDetailsLink() => lnkInspectionDetails.Click();
        public void ClickHowWillThisConsignmentBeTransportedLink() => lnkTransport.Click();
        public void ClickWhatAreThePackerDetailsLink() => lnkPackerDetails.Click();
        public void ClickCheckYourAnswersAndSubmitYourApplicationLink() => lnkCheckAnswers.Click();
    }
}