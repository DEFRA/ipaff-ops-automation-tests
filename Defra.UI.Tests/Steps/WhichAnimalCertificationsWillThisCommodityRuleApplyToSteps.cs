using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class WhichAnimalCertificationsWillThisCommodityRuleApplyToSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWhichAnimalCertificationsWillThisCommodityRuleApplyToPage? whichAnimalCertificationsWillThisCommodityRuleApplyToPage =>
            _objectContainer.IsRegistered<IWhichAnimalCertificationsWillThisCommodityRuleApplyToPage>()
                ? _objectContainer.Resolve<IWhichAnimalCertificationsWillThisCommodityRuleApplyToPage>()
                : null;

        public WhichAnimalCertificationsWillThisCommodityRuleApplyToSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Which animal certifications will this commodity rule apply to? page is displayed page should be displayed")]
        public void ThenTheWhichAnimalCertificationsWillThisCommodityRuleApplyToPageIsDisplayedPageShouldBeDisplayed()
        {
            Assert.True(whichAnimalCertificationsWillThisCommodityRuleApplyToPage?.IsPageLoaded(), "Which animal certifications will this commodity rule apply to? page is not displayed");
        }

        [When("the user ticks the {string} checkbox on the Which animal certifications will this commodity rule apply to? page")]
        public void WhenTheUserTicksTheCheckboxOnTheWhichAnimalCertificationsWillThisCommodityRuleApplyToPage(string checkboxLabel)
        {
            whichAnimalCertificationsWillThisCommodityRuleApplyToPage?.TickCheckbox(checkboxLabel);
        }

        [When("the user clicks the Continue button on the Which animal certifications will this commodity rule apply to? page")]
        public void WhenTheUserClicksTheContinueButtonOnTheWhichAnimalCertificationsWillThisCommodityRuleApplyToPage()
        {
            whichAnimalCertificationsWillThisCommodityRuleApplyToPage?.ClickContinueButton();
        }
    }
}