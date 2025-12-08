using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class AdditionalDetailsPage : IAdditionalDetailsPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement secondaryTitle => _driver.WaitForElement(By.Id("page-secondary-title"), true);
        private IWebElement rdoAmbient => _driver.WaitForElement(By.XPath("//*[@id='productTemperature']/following-sibling::label"));
        private IWebElement rdoChilled => _driver.WaitForElement(By.XPath("//*[@id='productTemperature-2']/following-sibling::label"));
        private IWebElement rdoFrozen => _driver.WaitForElement(By.XPath("//*[@id='productTemperature-3']/following-sibling::label"));
        private List<IWebElement> commIntendedForRadioList => _driver.WaitForElements(By.XPath("//div[@class='govuk-radios']/div[contains(@class,'commodity-intended-for')]/label")).ToList();
        private IWebElement getAnimalCertificationRadioButton(string certificationText) =>
            _driver.FindElement(By.XPath($"//label[@class='govuk-label govuk-radios__label' and normalize-space()='{certificationText}']"));
        private IWebElement rdoUnweanedAnimalsYes => _driver.FindElement(By.XPath("//*[@id='includingNonablactedyes']/following-sibling::label"));
        private IWebElement rdoUnweanedAnimalsNo => _driver.FindElement(By.XPath("//*[@id='includingNonAblactedno']/following-sibling::label"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public AdditionalDetailsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Description of the goods")
                && primaryTitle.Text.Contains("Additional details");
        }

        public bool IsAdditionalAnimalDetailsPageLoaded()
        {
            return secondaryTitle.Text.Contains("Description of the goods")
                && primaryTitle.Text.Contains("Additional animal details");
        }

        public void ClickImportingProduct(string option)
        {
            if(option.Equals(rdoAmbient.Text))
                rdoAmbient.Click();
            else if (option.Equals(rdoChilled.Text))
                rdoChilled.Click();
            else if (option.Equals(rdoFrozen.Text))
                rdoFrozen.Click();
        }

        public bool SelectCommodityIntendedForRadio(string commIntendedForOption)
        {
            foreach (var commIntendedForRadio in commIntendedForRadioList)
            {
                if (commIntendedForRadio.Text.Contains(commIntendedForOption))
                {
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", commIntendedForRadio);
                    return true;
                }
            }
            return false;
        }

        public void SelectAnimalCertification(string certificationOption)
        {
            var certificationRadioButton = getAnimalCertificationRadioButton(certificationOption);
            certificationRadioButton.Click();
        }

        public void SelectUnweanedAnimalsOption(string option)
        {
            if (option.Equals("Yes") || rdoUnweanedAnimalsYes.Text.Trim().Contains(option))
                rdoUnweanedAnimalsYes.Click();
            else if (option.Equals("No") || rdoUnweanedAnimalsNo.Text.Trim().Contains(option))
                rdoUnweanedAnimalsNo.Click();
        }
    }
}