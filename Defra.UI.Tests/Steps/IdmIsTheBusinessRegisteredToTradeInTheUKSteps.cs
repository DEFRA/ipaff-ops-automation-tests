using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmIsTheBusinessRegisteredToTradeInTheUKSteps
    {
        private readonly IObjectContainer _objectContainer;
        private IIdmIsTheBusinessRegisteredToTradeInTheUKPage idmIsTheBusinessRegisteredToTradeInTheUKPage => _objectContainer.Resolve<IIdmIsTheBusinessRegisteredToTradeInTheUKPage>();
        public IdmIsTheBusinessRegisteredToTradeInTheUKSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Is the business registered to trade in the UK? page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(idmIsTheBusinessRegisteredToTradeInTheUKPage.IsPageLoaded(), Is.True);

        [When("the user selects {string} on the Is the business registered to trade in the UK? page and continues")]
        public void WhenRegisteredCountryIsSelected(string option) => idmIsTheBusinessRegisteredToTradeInTheUKPage.SelectOptionAndContinue(option);
    }
}
