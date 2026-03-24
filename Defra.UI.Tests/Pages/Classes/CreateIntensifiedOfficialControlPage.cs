using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CreateIntensifiedOfficialControlPage : ICreateIntensifiedOfficialControlPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement txtCertificateNumber => _driver.FindElement(By.Id("certificate-number"));
        private IWebElement btnSearchForEstablishment => _driver.FindElement(By.Id("choose-approved-establishment"));
        private IWebElement txtSelectedEstablishmentName => _driver.FindElement(By.XPath("//*[@id='selected-establishment']/td[1]"));
        private IWebElement txtSelectedEstablishmentCountry => _driver.FindElement(By.XPath("//*[@id='selected-establishment']/td[2]"));
        private IWebElement txtSelectedEstablishmentApprovalNumber => _driver.FindElement(By.XPath("//*[@id='selected-establishment']/td[3]"));
        private IWebElement txtCommodityCode => _driver.FindElement(By.Id("commodity-code"));
        private IWebElement btnSearchCommodities => _driver.FindElement(By.Id("choose-commodity"));
        private IWebElement txtSelectedCommodityCode => _driver.FindElement(By.XPath("//*[@id='selected-commodity']/td[1]"));
        private IWebElement txtSelectedCommodityDescription => _driver.FindElement(By.XPath("//*[@id='selected-commodity']/td[2]"));
        private IWebElement btnSearchForHazard => _driver.FindElement(By.Id("choose-hazard"));
        private IWebElement txtSelectedHazardName => _driver.FindElement(By.XPath("//*[@id='selected-hazard']/td[1]"));
        private IWebElement txtNetWeight => _driver.FindElement(By.Id("net-weight"));
        private IWebElement btnPlaceUnderIntensifiedOfficialControls => _driver.FindElement(By.Id("submit-button"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public CreateIntensifiedOfficialControlPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        #region Methods

        public bool IsPageLoaded()
        {
            return pageTitle.Text.Contains("Create intensified official control");
        }

        public void EnterCertificateNumber(string certificateNumber)
        {
            txtCertificateNumber.Clear();
            txtCertificateNumber.SendKeys(certificateNumber);
        }

        public void ClickSearchForEstablishment()
        {
            btnSearchForEstablishment.Click();
        }

        public bool IsEstablishmentPopulated(string name, string country, string approvalNumber)
        {
            return txtSelectedEstablishmentName.Text.Contains(name, StringComparison.OrdinalIgnoreCase)
                && txtSelectedEstablishmentCountry.Text.Trim().Equals(country, StringComparison.OrdinalIgnoreCase)
                && txtSelectedEstablishmentApprovalNumber.Text.Trim().Equals(approvalNumber, StringComparison.OrdinalIgnoreCase);
        }

        public void EnterCommodityCode(string commodityCode)
        {
            txtCommodityCode.Clear();
            txtCommodityCode.SendKeys(commodityCode);
        }

        public void ClickSearchCommodities()
        {
            btnSearchCommodities.Click();
        }

        public bool IsCommodityPopulated(string code, string description)
        {
            return txtSelectedCommodityCode.Text.Trim().Equals(code, StringComparison.OrdinalIgnoreCase)
                && txtSelectedCommodityDescription.Text.Contains(description, StringComparison.OrdinalIgnoreCase);
        }

        public void ClickSearchForHazard()
        {
            btnSearchForHazard.Click();
        }

        public bool IsHazardPopulated(string hazardName)
        {
            return txtSelectedHazardName.Text.Trim().Equals(hazardName, StringComparison.OrdinalIgnoreCase);
        }

        public void EnterNetWeight(string netWeight)
        {
            txtNetWeight.Clear();
            txtNetWeight.SendKeys(netWeight);
        }

        public void ClickPlaceUnderIntensifiedOfficialControls()
        {
            btnPlaceUnderIntensifiedOfficialControls.Click();
        }

        #endregion
    }
}