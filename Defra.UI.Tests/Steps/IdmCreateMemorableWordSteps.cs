using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmCreateMemorableWordSteps
    {
        private readonly IObjectContainer _objectContainer;
        private IIdmCreateMemorableWordPage page => _objectContainer.Resolve<IIdmCreateMemorableWordPage>();
        public IdmCreateMemorableWordSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Create a memorable word page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(page.IsPageLoaded(), Is.True);

        [When("the user uses the configured Trader 1 credential as the memorable word and hint and continues")]
        public void WhenMemorableWordIsEntered()
        {
            var credential = _objectContainer.Resolve<IUserObject>().GetUser("IPAFF", "Trader 1").Credential;
            Assert.That(!string.IsNullOrWhiteSpace(credential) && !credential.Contains("#{{"), Is.True, "Trader 1 credential must be configured");
            page.EnterWordAndHintAndContinue(credential, credential);
        }
    }
}
