using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class EnterBorderNotificationDetailsPage : IEnterBorderNotificationDetailsPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement drpNotificationType => _driver.FindElement(By.Id("notification-type"));
        private IWebElement drpNotificationBasis => _driver.FindElement(By.Id("notification-basis"));
        private IWebElement drpProductCategory => _driver.FindElement(By.Id("product-category"));
        private IWebElement txtProductName => _driver.FindElement(By.Id("product-name"));
        private IWebElement txtBrandName => _driver.FindElement(By.Id("brand-name"));
        private IWebElement txtOtherLabel => _driver.FindElement(By.Id("other-labelling"));
        private IWebElement txtOtherInfo => _driver.FindElement(By.Id("other-information"));
        private IWebElement rdoDurabilityDate(string dateOption) => _driver.FindElement(By.XPath($"//label[normalize-space()='{dateOption}']"));
        private IWebElement drpRiskDecision => _driver.FindElement(By.Id("risk-decision"));
        private IWebElement drpImpactOn => _driver.FindElement(By.Id("impact-on"));
        private IWebElement drpHazardCategory => _driver.FindElement(By.Id("hazard-category"));
        private IWebElement drpMeasureTaken => _driver.FindElement(By.Id("measure-taken"));
        private IWebElement txtUseByDateDay => _driver.FindElement(By.Id("use-by-date-day"));
        private IWebElement txtUseByDateMonth => _driver.FindElement(By.Id("use-by-date-month"));
        private IWebElement txtUseByDateYear => _driver.FindElement(By.Id("use-by-date-year"));
        private IWebElement txtBestBeforeDay => _driver.FindElement(By.Id("best-before-date-day"));
        private IWebElement txtBestBeforeMonth => _driver.FindElement(By.Id("best-before-date-month"));
        private IWebElement txtBestBeforeYear => _driver.FindElement(By.Id("best-before-date-year"));
        private IWebElement txtBestBeforeEndMonth => _driver.FindElement(By.Id("best-before-end-date-month"));
        private IWebElement txtBestBeforeEndYear => _driver.FindElement(By.Id("best-before-end-date-year"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();
        private readonly object @lock = new();

        public EnterBorderNotificationDetailsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Enter the details of the border notification");
        }

        public void SelectNotificationDetails(string type, string basis)
        {
            new SelectElement(drpNotificationType).SelectByText(type);
            new SelectElement(drpNotificationBasis).SelectByText(basis);
        }

        public void SelectProductDetails(string category, string product, string brand)
        {
            new SelectElement(drpProductCategory).SelectByText(category);
            txtProductName.Click();
            txtProductName.SendKeys(product);
            txtBrandName.Click();
            txtBrandName.SendKeys(brand);
        }

        public void SelectOtherDetails(string label, string otherInfo, string dateOption)
        {
            txtOtherLabel.Click();
            txtOtherLabel.SendKeys(label);
            txtOtherInfo.Click();
            txtOtherInfo.SendKeys(otherInfo);
            rdoDurabilityDate(dateOption).Click();

            var futureDate = DateTime.Now.AddDays(20);
            var day = futureDate.ToString("dd");
            var month = futureDate.ToString("MM");
            var year = futureDate.ToString("yyyy");

            if (dateOption.Equals("Use by date"))
            {
                txtUseByDateDay.Click();
                txtUseByDateDay.SendKeys(day);
                txtUseByDateMonth.Click();
                txtUseByDateMonth.SendKeys(month);
                txtUseByDateYear.Click();
                txtUseByDateYear.SendKeys(year);
            }
            else if (dateOption.Equals("Best before date"))
            {
                txtBestBeforeDay.Click();
                txtBestBeforeDay.SendKeys(day);
                txtBestBeforeMonth.Click();
                txtBestBeforeMonth.SendKeys(month);
                txtBestBeforeYear.Click();
                txtBestBeforeYear.SendKeys(year);
            }
            else if (dateOption.Equals("Best before end"))
            {
                txtBestBeforeEndMonth.Click();
                txtBestBeforeEndMonth.SendKeys(month);
                txtBestBeforeEndYear.Click();
                txtBestBeforeEndYear.SendKeys(year);
            }
        }

        public void SelectRiskDetails(string decision, string impact, string hazard, string measure)
        {
            new SelectElement(drpRiskDecision).SelectByText(decision);
            new SelectElement(drpImpactOn).SelectByText(impact);
            new SelectElement(drpHazardCategory).SelectByText(hazard);
            new SelectElement(drpMeasureTaken).SelectByText(measure);
        }
    }
}