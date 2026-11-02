using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmDoYouHaveACompanyRegistrationNumberSteps
    {
        private readonly IObjectContainer _objectContainer;
        private IIdmDoYouHaveACompanyRegistrationNumberPage page => _objectContainer.Resolve<IIdmDoYouHaveACompanyRegistrationNumberPage>();
        public IdmDoYouHaveACompanyRegistrationNumberSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Do you have a company registration number? page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(page.IsPageLoaded(), Is.True);

        [When("the user selects {string} on the Do you have a company registration number? page and continues")]
        public void WhenCompanyRegistrationOptionIsSelected(string option) => page.SelectOptionAndContinue(option);
    }
}
