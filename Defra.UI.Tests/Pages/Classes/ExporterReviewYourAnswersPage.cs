using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ExporterReviewYourAnswersPage : IExporterReviewYourAnswersPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects with all the element locators
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[normalize-space()='Review your answers']"), true);
        private IWebElement btnContinue => _driver.WaitForElement(By.Id("Button-Continue"));
        private IWebElement tblCommodities => _driver.WaitForElement(By.Id("commodity-table"), true);
        private IWebElement lblConsignmentValue => _driver.WaitForElement(By.Id("transport-consignment-value"), true);
        private IWebElement lblPointOfEntry => _driver.WaitForElement(By.Id("transport-point-of-entry-value"), true);
        private IWebElement lblTransportMode => _driver.WaitForElement(By.Id("transport-mode-value"), true);
        private IWebElement lblDestinationCountry => _driver.WaitForElement(By.XPath("//div[@id='destination-country']/dd[contains(@class,'govuk-summary-list__value')]"), true);
        private IWebElement lblYourReference => _driver.WaitForElement(By.XPath("//dl[@id='application-summary-section']//dt[normalize-space()='Your reference']/following-sibling::dd[1]"), true);
        private IWebElement lblCommodityDescription => _driver.WaitForElement(By.XPath("//table[@id='commodity-table']/tbody/tr[1]/td[1]/strong"), true);
        private IWebElement lblCommodityVariety => _driver.WaitForElement(By.XPath("//table[@id='commodity-table']/tbody/tr[1]/td[1]/div[1]"), true);
        private IWebElement lblCommodityLotReference => _driver.WaitForElement(By.XPath("//table[@id='commodity-table']/tbody/tr[1]/td[1]/div[2]"), true);
        private IWebElement lblCommodityType => _driver.WaitForElement(By.XPath("//table[@id='commodity-table']/tbody/tr[1]/td[1]/div[contains(normalize-space(),'Commodity type:')]"), true);
        private IWebElement lblCommodityOrigin => _driver.WaitForElement(By.XPath("//table[@id='commodity-table']/tbody/tr[1]/td[2]"), true);
        private IWebElement lblCommodityQuantity => _driver.WaitForElement(By.XPath("//table[@id='commodity-table']/tbody/tr[1]/td[3]/div[1]"), true);
        private IWebElement lblCommodityNumberOfPackages => _driver.WaitForElement(By.XPath("//table[@id='commodity-table']/tbody/tr[1]/td[3]/div[2]"), true);
        private IWebElement lblCommodityPackaging => _driver.WaitForElement(By.XPath("//table[@id='commodity-table']/tbody/tr[1]/td[4]/div[1]"), true);
        private IWebElement lblConsigneeDetails => _driver.WaitForElement(By.XPath("//dl[@id='consignee-details-section']/div[1]/dd[contains(@class,'govuk-summary-list__value')]"), true);
        private IWebElement lblExporterName => _driver.WaitForElement(By.Id("nameValue"), true);
        private IWebElement lblExporterAddress => _driver.WaitForElement(By.Id("addressValue"), true);
        private IWebElement lblSupportingDocumentFileName => _driver.WaitForElement(By.Id("file_0"), true);
        private IWebElement lblSupportingDocumentDescription => _driver.WaitForElement(By.Id("description_0"), true);
        #endregion

        public ExporterReviewYourAnswersPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() => pageTitle.Text.Trim().Equals("Review your answers");
        public void ClickContinueButton() => btnContinue.Click();
        public string GetCommodityTableText() => System.Text.RegularExpressions.Regex.Replace(tblCommodities.Text, @"\s+", " ").Trim();
        public string GetConsignmentValue() => lblConsignmentValue.Text.Trim();
        public string GetPointOfEntry() => lblPointOfEntry.Text.Trim();
        public string GetTransportMode() => lblTransportMode.Text.Trim();
        public string GetDestinationCountry() => lblDestinationCountry.Text.Trim();
        public string GetYourReference() => lblYourReference.Text.Trim();
        public string GetCommodityDescription() => lblCommodityDescription.Text.Trim();
        public string GetCommodityVariety() => lblCommodityVariety.Text.Trim();
        public string GetCommodityLotReference() => lblCommodityLotReference.Text.Replace("Lot reference:", "").Trim();
        public string GetCommodityType() => lblCommodityType.Text.Replace("Commodity type:", "").Trim();
        public string GetCommodityOrigin() => lblCommodityOrigin.Text.Trim();
        public string GetCommodityQuantity() => lblCommodityQuantity.Text.Trim();
        public string GetCommodityNumberOfPackages() => lblCommodityNumberOfPackages.Text.Trim();
        public string GetCommodityPackaging() => lblCommodityPackaging.Text.Trim();
        public string GetConsigneeName() => GetConsigneeLines().FirstOrDefault() ?? string.Empty;
        public string GetConsigneeAddress() => GetConsigneeLines().Skip(1).FirstOrDefault() ?? string.Empty;
        public string GetExporterName() => lblExporterName.Text.Trim();
        public string GetExporterAddress() => lblExporterAddress.Text.Trim();
        public string GetSupportingDocumentFileName() => lblSupportingDocumentFileName.Text.Trim();
        public string GetSupportingDocumentDescription() => lblSupportingDocumentDescription.Text.Replace('\u00A0', ' ').Trim();

        public void ClickChangeLink(string fieldName)
        {
            // Supporting files are listed in a table headed 'File name' rather than a summary list key
            var locator = fieldName.Trim().Equals("File name", StringComparison.OrdinalIgnoreCase)
                ? By.Id("change_file_0")
                : By.XPath($"//dt[normalize-space()='{fieldName}']/following-sibling::dd[contains(@class,'govuk-summary-list__actions')][1]//a[starts-with(normalize-space(),'Change')]");
            _driver.WaitForElement(locator).Click();
        }

        private string[] GetConsigneeLines() =>
            lblConsigneeDetails.Text.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
}