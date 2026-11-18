using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmYourDefraAccountTermsAndConditionsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private IIdmYourDefraAccountTermsAndConditionsPage idmYourDefraAccountTermsAndConditionsPage => _objectContainer.Resolve<IIdmYourDefraAccountTermsAndConditionsPage>();
        public IdmYourDefraAccountTermsAndConditionsSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Your Defra account terms and conditions page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(idmYourDefraAccountTermsAndConditionsPage.IsPageLoaded(), Is.True);

        [When("the user clicks Continue on the Your Defra account terms and conditions page")]
        public void WhenTermsAreAccepted() => idmYourDefraAccountTermsAndConditionsPage.AcceptAndContinue();
    }
}
