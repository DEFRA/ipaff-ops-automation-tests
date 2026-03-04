using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class PermanentAddressesPage : IPermanentAddressesPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//*[@id='page-primary-title']"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.XPath("//*[@id='page-secondary-title']"), true);
        private IReadOnlyCollection<IWebElement> animalPanels => _driver.FindElements(By.XPath("//div[contains(@class,'panel--animal-permanent-address')]"));
        private IReadOnlyCollection<IWebElement> animalNameHeadings => _driver.FindElements(By.XPath("//h2[starts-with(@id, 'animal-name-')]"));
        private IWebElement btnSaveAndContinue => _driver.FindElement(By.Id("button-save-and-continue"));
        private IWebElement btnSaveAndReturnToHub => _driver.FindElement(By.Id("save-and-return-button"));

        private IWebElement rdoSameAsPod(int index) =>
            _driver.FindElement(By.XPath($"//input[@id='same-address-as-pod-{index}']"));

        private IWebElement rdoDifferentAddress(int index) =>
            _driver.FindElement(By.XPath($"//input[@id='different-address-from-pod-{index}']"));

        private IWebElement txtAddressName(int index) =>
            _driver.FindElement(By.Id($"address-name-{index}"));

        private IWebElement txtAddressLine1(int index) =>
            _driver.FindElement(By.Id($"address-line-1-{index}"));

        private IWebElement txtAddressLine2(int index) =>
            _driver.FindElement(By.Id($"address-line-2-{index}"));

        private IWebElement txtCityOrTown(int index) =>
            _driver.FindElement(By.Id($"city-or-town-{index}"));

        private IWebElement txtPostcode(int index) =>
            _driver.FindElement(By.Id($"postcode-{index}"));

        private IWebElement txtTelephone(int index) =>
            _driver.FindElement(By.Id($"telephone-{index}"));

        private IWebElement txtEmail(int index) =>
            _driver.FindElement(By.Id($"email-{index}"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public PermanentAddressesPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Traders")
                && primaryTitle.Text.Contains("Permanent addresses for these animals");
        }

        /// <summary>
        /// Selects the same address option for all animals displayed on the page.
        /// <param name="option">Either "Same as the place of destination (POD)" or "A different address"</param>
        /// </summary>
        public void SelectAddressOptionForAllAnimals(string option)
        {
            var count = animalPanels.Count;

            for (int i = 0; i < count; i++)
            {
                if (option.Equals("A different address", StringComparison.OrdinalIgnoreCase))
                    rdoDifferentAddress(i).Click(_driver);
                else
                    rdoSameAsPod(i).Click(_driver);
            }
        }

        /// <summary>
        /// Enters generated permanent address details for the animal at the given 0-based index.
        /// Assumes "A different address" radio has already been selected for this animal,
        /// which reveals the conditional address fields.
        /// </summary>
        public void EnterPermanentAddressDetails(int animalIndex)
        {
            var details = Utils.GenerateOperatorDetails("Importer");
            PopulateAddressFields(animalIndex, details);
        }

        /// <summary>
        /// Finds the animal panel by matching the heading text "{species} {animalIndex}" (e.g. "Canis familiaris 1"),
        /// extracts the 0-based HTML index from the heading id (e.g. "animal-name-0" → 0),
        /// generates address details, populates the form fields, and returns the generated details.
        /// </summary>
        public OperatorDetails EnterPermanentAddressForAnimal(string species, int animalIndex)
        {
            var htmlIndex = FindHtmlIndexForAnimal(species, animalIndex);
            var details = Utils.GenerateOperatorDetails("Importer");
            PopulateAddressFields(htmlIndex, details);
            return details;
        }

        public void ClickSaveAndContinue()
        {
            btnSaveAndContinue.Click();
        }

        public void ClickSaveAndReturnToHub()
        {
            btnSaveAndReturnToHub.Click();
        }

        /// <summary>
        /// Finds the 0-based HTML index for a given species and animal number by matching
        /// the heading text (e.g. "Canis familiaris 1") against the h2 elements with id="animal-name-{n}".
        /// </summary>
        private int FindHtmlIndexForAnimal(string species, int animalIndex)
        {
            var expectedText = $"{species} {animalIndex}";
            var heading = animalNameHeadings
                .FirstOrDefault(h => h.Text.Trim().Equals(expectedText, StringComparison.OrdinalIgnoreCase))
                ?? throw new NoSuchElementException($"Animal panel heading not found for '{expectedText}'");

            var id = heading.GetAttribute("id");
            // id is "animal-name-{n}", extract the numeric suffix
            var indexStr = id.Replace("animal-name-", "");
            return int.Parse(indexStr);
        }

        private void PopulateAddressFields(int htmlIndex, OperatorDetails details)
        {
            txtAddressName(htmlIndex).Clear();
            txtAddressName(htmlIndex).SendKeys(details.OperatorName);

            txtAddressLine1(htmlIndex).Clear();
            txtAddressLine1(htmlIndex).SendKeys(details.AddressLine1);

            txtCityOrTown(htmlIndex).Clear();
            txtCityOrTown(htmlIndex).SendKeys(details.CityOrTown);

            txtPostcode(htmlIndex).Clear();
            txtPostcode(htmlIndex).SendKeys(details.Postcode);

            txtTelephone(htmlIndex).Clear();
            txtTelephone(htmlIndex).SendKeys(details.TelephoneNumber);

            txtEmail(htmlIndex).Clear();
            txtEmail(htmlIndex).SendKeys(details.Email);
        }
    }
}