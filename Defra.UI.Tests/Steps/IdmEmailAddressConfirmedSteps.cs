using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmEmailAddressConfirmedSteps
    {
        private readonly IObjectContainer _objectContainer;
        private IIdmEmailAddressConfirmedPage idmEmailAddressConfirmedPage => _objectContainer.Resolve<IIdmEmailAddressConfirmedPage>();
        public IdmEmailAddressConfirmedSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Email address confirmed page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(idmEmailAddressConfirmedPage.IsPageLoaded(), Is.True);

        [When("the user continues from the Email address confirmed page")]
        public void WhenContinueIsClicked() => idmEmailAddressConfirmedPage.ClickContinueButton();
    }
}
