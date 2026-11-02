using Defra.UI.Tests.Pages.Interfaces;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmSignInUsingGovernmentGatewaySteps
    {
        private readonly IObjectContainer _objectContainer;
        public IdmSignInUsingGovernmentGatewaySteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [When("the user clicks Create sign in details on the Government Gateway sign in page")]
        public void WhenCreateSignInDetailsIsClicked() => _objectContainer.Resolve<ISignInPage>().ClickCreateSignInDetailsLink();
    }
}
