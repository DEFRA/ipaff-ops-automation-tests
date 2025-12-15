using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class ChecksSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IChecksPage? checksPage => _objectContainer.IsRegistered<IChecksPage>() ? _objectContainer.Resolve<IChecksPage>() : null;

        public ChecksSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Checks page should be displayed")]
        public void ThenTheChecksPageShouldBeDisplayed()
        {
            Assert.True(checksPage?.IsChecksPageLoaded(), "Checks page is not displayed");
        }

        [When("the user selects {string} radio button under Documentary check on the Checks page")]
        public void WhenTheUserSelectsRadioButtonUnderDocumentaryCheckOnTheChecksPage(string docCheckOption)
        {
            checksPage?.SelectDocCheckRadio(docCheckOption);
            _scenarioContext.Add("DocumentaryCheckDecision", docCheckOption);
        }

        [When("the user selects {string} radio button under Identity check on the Checks page")]
        public void WhenTheUserSelectsRadioButtonUnderIdentityCheckOnTheChecksPage(string identityCheckOption)
        {
            checksPage?.SelectIdentityCheckRadio(identityCheckOption);
        }

        [When("the user selects {string} sub radio button under the Identity check main radio")]
        public void WhenTheUserSelectsSubRadioButtonUnderTheIdentityCheckMainRadio(string identityCheckSubOption)
        {
            checksPage?.SelectIdentityCheckSubRadio(identityCheckSubOption);
            _scenarioContext.Add("IdentityCheckDecision", identityCheckSubOption);
        }

        [When("the user selects {string} radio button under Physical check on the Checks page")]
        public void WhenTheUserSelectsRadioButtonUnderPhysicalCheckOnTheChecksPage(string physicalCheckOption)
        {
            checksPage?.SelectPhysicalCheckRadio(physicalCheckOption);
        }

        [When("the user selects {string} sub radio button under the Physical check main radio")]
        public void WhenTheUserSelectsSubRadioButtonUnderThePhysicalCheckMainRadio(string physicalCheckSubOption)
        {
            checksPage?.SelectPhysicalCheckSubRadio(physicalCheckSubOption);
            _scenarioContext.Add("PhysicalCheckDecision", physicalCheckSubOption);
        }

        [When("the user clicks on Save and continue button on the Checks page")]
        public void WhenTheUserClicksOnSaveAndContinueButtonOnTheChecksPage()
        {
            checksPage?.ClickSaveAndContinueButton();
        }
    }
}