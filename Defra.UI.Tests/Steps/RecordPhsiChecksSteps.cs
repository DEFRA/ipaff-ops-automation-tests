using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class RecordPhsiChecksSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IRecordPhsiChecksPage? recordPhsiChecksPage =>
            _objectContainer.IsRegistered<IRecordPhsiChecksPage>()
                ? _objectContainer.Resolve<IRecordPhsiChecksPage>()
                : null;

        public RecordPhsiChecksSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Record PHSI checks page should be displayed")]
        [Then("the Choose commodities to record decisions against page should be displayed")]
        public void ThenTheChooseCommoditiesToRecordDecisionsAgainstPageShouldBeDisplayed()
        {
            Assert.True(
                recordPhsiChecksPage?.IsPageLoaded(),
                "Record PHSI checks / Choose commodities to record decisions against page was not loaded.");
        }

        [When("the user records {string} for all documentary, identity and physical checks across all pages")]
        public void WhenTheUserRecordsForAllChecksAcrossAllPages(string decision)
        {
            recordPhsiChecksPage?.RecordAllCompliantDecisionsAcrossAllPages(decision);
        }
    }
}