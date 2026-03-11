using Defra.UI.Tests.Contracts;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class CertificateDetailsPage : ICertificateDetailsPage
    {
        private IObjectContainer _objectContainer;
        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        #region Page Objects
        private IWebElement PageHeading => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IReadOnlyCollection<IWebElement> divSubTitles => _driver.FindElements(By.XPath("//label[@class='govuk-label govuk-label--m']"));
        private IWebElement legendSubTitle => _driver.FindElement(By.XPath("//legend[contains(@class,'govuk-fieldset__legend--m')]"));
        private IWebElement drpContryOfOrigin => _driver.FindElement(By.Id("country-of-origin"));
        private IWebElement txtCerfificateRefNumber => _driver.FindElement(By.Id("certificate-reference"));
        private IWebElement txtCertificateIssueDay => _driver.FindElement(By.Id("certificate-date-of-issue-day"));
        private IWebElement txtCertificateIssueMonth => _driver.FindElement(By.Id("certificate-date-of-issue-month"));
        private IWebElement txtCertificateIssueYear => _driver.FindElement(By.Id("certificate-date-of-issue-year"));
        private IWebElement txtConsignorConsigneeImporterName => _driver.FindElement(By.Id("party-name"));
        private IWebElement btnSearch => _driver.FindElement(By.Id("search"));
        private IWebElement healthCertificatePageHeading => _driver.FindElement(By.Id("page-primary-title"));

        #endregion

        public CertificateDetailsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return PageHeading.Text.Contains("Certificate details");
        }

        public bool VerifyCertificateDetailsPageSubtitles()
        {
            return
                Utils.CollectionsEqualIgnoreOrder(
                    divSubTitles.Select(x => x.Text),
                    [
                        "Country of origin of certificate",
                        "Certificate reference number",
                        "Consignor, consignee or importer name"
                    ]
                )
                &&
                legendSubTitle.Text.TextEquals("Search for a certificate to clone");

        }

        public void SelectCountryOfOrigin(string countryOdOrigin)
        {
            drpContryOfOrigin.SelectByText(countryOdOrigin);
        }

        public void EnterCertificateReferenceNumber(string referencNumber)
        {
            txtCerfificateRefNumber.SendKeys(referencNumber);
        }

        public void EnterConsignerConsigneeImporterName(string name)
        {
            txtConsignorConsigneeImporterName.Clear();
            txtConsignorConsigneeImporterName.SendKeys(name);
        }

        public NotificationDetails GetNotificationDetailsForCloningCHEDPP()
        {
            return new NotificationDetails
            {
                CertificateReferenceNumber = "PHYTO.FR.2026.0000004",
                CertificateDateOfIssue = "12/01/2026",
                CountryOfOriginOfCertificate = "France",
                ConsignorConsigneeOrImporterName = "LIBONX28 WW",
                PurposeOfTheConsignment= "For Import",
                CommodityCode= "06011010",
                Description= "Hyacinths",
                GenusAndSpecies= "Bellevalia trifoliata",
                NetWeightWithUnits = "1000.0 kgs/units",
                NetWeight="1000",
                Packages="10",
                TypeOfPackage= "Box",
                Quantity= "1000.0",
                QuantityType= "Kilograms",
                DocumentType = "Phytosanitary certificate"
            };
        }

        public NotificationDetails GetNotificationDetailsForCloningCHEDP()
        {
            return new NotificationDetails
            {
                CertificateReferenceNumber = "NZL2026/AGL18/1",
                CertificateDateOfIssue = "15/01/2026",
                CountryOfOriginOfCertificate = "New Zealand",
                ConsignorConsigneeOrImporterName = "ALLIANCE GROUP (NZ) LTD",
                PurposeOfTheConsignment = "Human Consumption",
                CommodityCode = "02044210",
                Description = "Short forequarters",
                GenusAndSpecies = "Ovis aries",
                NetWeightWithUnits = "25200.9 kgs/units",
                NetWeight = "25200.9",
                Packages = "1255",
                TypeOfPackage = "Carton",
                Temperature = "Frozen",
                Container = "MNBU4285672",
                SealNumber= "NZMPI01354359",
                DocumentType= "Veterinary health certificate"
            };
        }

        public void EnterCertificateDateOfIssueYear(int day, int month, int year)
        {
            txtCertificateIssueDay.Clear();
            txtCertificateIssueDay.SendKeys(day.ToString());
            txtCertificateIssueMonth.Clear();
            txtCertificateIssueMonth.SendKeys(month.ToString());
            txtCertificateIssueYear.Clear();
            txtCertificateIssueYear.SendKeys(year.ToString());
        }

        public void ClickSearchButton()
        {
            btnSearch.Click();
        }

    }
}
