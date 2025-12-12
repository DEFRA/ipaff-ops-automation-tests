using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class LaboratoryTestsPage : ILaboratoryTestsPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement labTestsRadio(string labTestsOption) => _driver.FindElement(By.XPath($"//input[@class='govuk-radios__input']/following-sibling::label[contains(text(),'{labTestsOption}')]"));
        private IWebElement rdoLabTestsYes => _driver.FindElement(By.Id("radio-lab-tests-required-yes"));
        private IWebElement rdoLabTestsNo => _driver.FindElement(By.Id("radio-lab-tests-required-no"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public LaboratoryTestsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Laboratory tests");
        }

        public void SelectLabTestsRadio(string labTestsOption)
        {
            labTestsRadio(labTestsOption).Click();
        }

        public bool IsLabTestsNoPreselected()
        {
            return rdoLabTestsNo.GetAttribute("checked") != null;
        }
    }
}