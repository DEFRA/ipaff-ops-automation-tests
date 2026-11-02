using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmYourGovernmentGatewayUserIdIsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private IIdmYourGovernmentGatewayUserIdIsPage page => _objectContainer.Resolve<IIdmYourGovernmentGatewayUserIdIsPage>();
        public IdmYourGovernmentGatewayUserIdIsSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Your Government Gateway user ID is: page is displayed")]
        public void ThenPageIsDisplayed()
        {
            Assert.That(page.IsPageLoaded(), Is.True);
            Assert.That(page.GetGatewayId(), Does.Match(@"^\d{12}$"), "Expected a 12-digit Government Gateway ID");
        }

        [When("the user continues from the Your Government Gateway user ID is: page")]
        public void WhenContinueIsClicked() => page.ClickContinueButton();
    }
}
