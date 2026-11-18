using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmCreatePasswordSteps
    {
        private readonly IObjectContainer _objectContainer;
        private IIdmCreatePasswordPage idmCreatePasswordPage => _objectContainer.Resolve<IIdmCreatePasswordPage>();
        public IdmCreatePasswordSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Create a password page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(idmCreatePasswordPage.IsPageLoaded(), Is.True);

        [When("the user enters a password and continues")]
        public void WhenPasswordIsEntered()
        {
            var credential = _objectContainer.Resolve<IUserObject>().GetUser("IPAFF", "Trader 1").Credential;
            Assert.That(!string.IsNullOrWhiteSpace(credential) && !credential.Contains("#{{"), Is.True, "Trader 1 credential must be configured");
            idmCreatePasswordPage.EnterPasswordAndContinue(credential);
        }
    }
}
