using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmTermsAndConditionsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private IIdmTermsAndConditionsPage page => _objectContainer.Resolve<IIdmTermsAndConditionsPage>();
        public IdmTermsAndConditionsSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the exporter registration Terms and conditions page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(page.IsPageLoaded(), Is.True);

        [When("the user accepts the exporter registration terms and continues")]
        public void WhenTermsAreAccepted() => page.AcceptAndContinue();
    }
}
