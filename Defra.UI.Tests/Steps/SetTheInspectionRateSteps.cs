using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class SetTheInspectionRateSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ISetTheInspectionRatePage? setTheInspectionRatePage =>
            _objectContainer.IsRegistered<ISetTheInspectionRatePage>()
                ? _objectContainer.Resolve<ISetTheInspectionRatePage>()
                : null;

        public SetTheInspectionRateSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Set the inspection rate page should be displayed")]
        public void ThenTheSetTheInspectionRatePageShouldBeDisplayed()
        {
            Assert.True(setTheInspectionRatePage?.IsPageLoaded(), "Set the inspection rate page is not displayed");
        }

        [When("the user enters {string} as the inspection rate on the Set the inspection rate page")]
        public void WhenTheUserEntersAsTheInspectionRateOnTheSetTheInspectionRatePage(string rate)
        {
            setTheInspectionRatePage?.EnterInspectionRate(rate);
        }

        [When("the user clicks the Continue button on the Set the inspection rate page")]
        public void WhenTheUserClicksTheContinueButtonOnTheSetTheInspectionRatePage()
        {
            setTheInspectionRatePage?.ClickContinueButton();
        }
    }
}