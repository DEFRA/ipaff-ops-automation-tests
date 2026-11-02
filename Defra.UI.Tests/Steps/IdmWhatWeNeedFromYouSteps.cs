using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmWhatWeNeedFromYouSteps
    {
        private readonly IObjectContainer _objectContainer;
        private IIdmWhatWeNeedFromYouPage page => _objectContainer.Resolve<IIdmWhatWeNeedFromYouPage>();
        public IdmWhatWeNeedFromYouSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the What we need from you page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(page.IsPageLoaded(), Is.True);

        [When("the user continues from the What we need from you page")]
        public void WhenContinueIsClicked() => page.ClickContinueButton();
    }
}
