using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class RiskCategorySteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

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
            _scenarioContext["RiskCategory"] = option;
        }
    }
}