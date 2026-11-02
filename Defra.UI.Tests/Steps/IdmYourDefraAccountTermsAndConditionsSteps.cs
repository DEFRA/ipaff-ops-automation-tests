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
        private IIdmYourDefraAccountTermsAndConditionsPage page => _objectContainer.Resolve<IIdmYourDefraAccountTermsAndConditionsPage>();
        public IdmYourDefraAccountTermsAndConditionsSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Your Defra account terms and conditions page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(page.IsPageLoaded(), Is.True);

        [When("the user accepts the Defra account terms and continues")]
        public void WhenTermsAreAccepted() => page.AcceptAndContinue();
    }
}
