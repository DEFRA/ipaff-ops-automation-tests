using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmYouNeedToRegisterForADefraAccountSteps
    {
        private readonly IObjectContainer _objectContainer;
        private IIdmYouNeedToRegisterForADefraAccountPage page => _objectContainer.Resolve<IIdmYouNeedToRegisterForADefraAccountPage>();
        public IdmYouNeedToRegisterForADefraAccountSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the You need to register for a Defra account page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(page.IsPageLoaded(), Is.True);

        [When("the user continues registering for a Defra account")]
        public void WhenContinueIsClicked() => page.ClickContinueButton();
    }
}
