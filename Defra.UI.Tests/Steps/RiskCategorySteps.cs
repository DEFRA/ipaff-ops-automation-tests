using Reqnroll.BoDi;
using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class RiskCategorySteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private IRiskCategoryPage? riskCategoryPage => _objectContainer.IsRegistered<IRiskCategoryPage>() ? _objectContainer.Resolve<IRiskCategoryPage>() : null;


        public RiskCategorySteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("Select the highest risk category for the commodities in this consignment page should be displayed")]
        public void ThenTheAboutTheConsignmentWhatAreYouImportingPageShouldBeDisplayedWithRadioButtons()
        {
            Assert.True(riskCategoryPage?.IsPageLoaded(), "Select the highest risk category for the commodities in this consignment");
        }

        [When("the user chooses {string} risk category")]
        public void WhenTheUserChoosesRiskCategory(string option)
        {
            riskCategoryPage?.ClickRiskCategory(option);
            _scenarioContext.Add("RiskCategory", option);
        }
    }
}