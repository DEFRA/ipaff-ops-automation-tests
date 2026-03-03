using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using SeleniumExtras.WaitHelpers;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System.Collections.ObjectModel;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;

namespace Defra.UI.Tests.Pages.Classes
{
    public class AnimalIdentificationDetailsPage : IAnimalIdentificationDetailsPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.XPath("//*[@id='page-primary-title']"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.XPath("//*[@id='page-secondary-title']"), true);
        private IWebElement txtIdentificationDetails => _driver.FindElement(By.XPath("//input[contains(@id, 'identification_details')]"));
        private IWebElement txtDescription => _driver.FindElement(By.XPath("//input[contains(@id, 'description')]"));
        private IWebElement txtHorseName => _driver.FindElement(By.XPath("//input[contains(@id, 'horse_name')]"));
        private IWebElement txtMicrochipNumber => _driver.FindElement(By.XPath("//input[contains(@id, 'microchip')]"));
        private IWebElement txtPassportNumber => _driver.FindElement(By.XPath("//input[contains(@id, 'passport')]"));
        private IWebElement txtEarTag => _driver.FindElement(By.XPath("//input[contains(@id, 'ear_tag')]"));
        private IWebElement lblNumberOfAnimals => _driver.FindElement(By.XPath("//table[@id='commodities-overview']//th[text()='Number of animals']/parent::tr/parent::thead/following-sibling::tbody//td[4]"));
        private IReadOnlyCollection<IWebElement> identifiersTables => _driver.FindElements(By.XPath("//table[contains(@class, 'identifiers-table')]"));
        private IWebElement identifierInput(string sectionId, int animalIndex, string fieldType) =>
            _driver.WaitForElement(By.XPath($"//input[@id='{sectionId}.identifiers-{animalIndex}-{fieldType}']"));
        private IWebElement addAnotherButton(string sectionId, int currentAnimalCount) =>
            _driver.WaitForElement(By.XPath($"//button[@id='add-{sectionId}.identifiers-{currentAnimalCount}']"));
        private IReadOnlyCollection<IWebElement> speciesInputRows(string sectionId) =>
            _driver.FindElements(By.XPath($"//table[@id='identifiers-table-{sectionId}']//tbody/tr[contains(@class,'input-row')]"));
        private By speciesHeadingBy(string species) =>
            By.XPath($"//h2[@id and contains(@class,'govuk-heading-m') and contains(normalize-space(.), '{species}')]");
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public AnimalIdentificationDetailsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded() =>
            secondaryTitle.Text.Contains("Description of the goods")
            && primaryTitle.Text.Contains("Enter animal identification details");

        public void EnterIdentificationDetails(string identificationDetails)
        {
            txtIdentificationDetails.Clear();
            txtIdentificationDetails.SendKeys(identificationDetails);
        }

        public void EnterDescription(string description)
        {
            txtDescription.Clear();
            txtDescription.SendKeys(description);
        }

        public void EnterHorseName(string horseName)
        {
            txtHorseName.Clear();
            txtHorseName.SendKeys(horseName);
        }

        public void EnterMicrochipNumber(string microchipNumber)
        {
            txtMicrochipNumber.Clear();
            txtMicrochipNumber.SendKeys(microchipNumber);
        }

        public void EnterPassportNumber(string passportNumber)
        {
            txtPassportNumber.Clear();
            txtPassportNumber.SendKeys(passportNumber);
        }

        public void EnterEarTag(string earTag)
        {
            txtEarTag.Clear();
            txtEarTag.SendKeys(earTag);
        }

        public string GetEarTag => txtEarTag.GetAttribute("value")?.Trim() ?? string.Empty;

        public string GetNumberOfAnimals() => lblNumberOfAnimals.Text.Trim();

        public List<string> GetSpeciesSectionIds() =>
            identifiersTables
                .Select(t => t.GetAttribute("id"))
                .Where(id => !string.IsNullOrEmpty(id))
                .Select(id => id.Replace("identifiers-table-", ""))
                .ToList();

        public void EnterIdentificationForSpecies(string species, int animalIndex, string fieldType, string value)
        {
            var sectionId = FindSectionIdForSpecies(species);
            var input = identifierInput(sectionId, animalIndex, fieldType);
            input.Clear();
            input.SendKeys(value);
        }

        public void ClickAddAnotherForSpecies(string species)
        {
            var sectionId = FindSectionIdForSpecies(species);
            var button = addAnotherButton(sectionId, speciesInputRows(sectionId).Count);
            button.Click();
        }        

        /// <summary>
        /// Finds the section ID (e.g. "1-568113") for a given species name.
        /// Uses a WebDriverWait with a custom condition that locates the heading AND
        /// reads its @id attribute atomically in a single evaluation, retrying on
        /// StaleElementReferenceException. This handles the page re-render that occurs
        /// after "Add another" (type="submit") causes the DOM to be replaced.
        /// </summary>
        private string FindSectionIdForSpecies(string species)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

            var sectionId = wait.Until(driver =>
            {
                var heading = driver.FindElement(speciesHeadingBy(species));
                var id = heading.GetAttribute("id");
                return string.IsNullOrEmpty(id) ? null : id;
            });

            return sectionId
                ?? throw new NoSuchElementException($"Species section heading not found for '{species}'");
        }
    }
}