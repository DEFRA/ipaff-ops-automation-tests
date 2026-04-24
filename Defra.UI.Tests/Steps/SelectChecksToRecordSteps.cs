using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class SelectChecksToRecordSteps
    {
        private readonly IObjectContainer _objectContainer;

        private ISelectChecksToRecordPage? selectChecksToRecordPage =>
            _objectContainer.IsRegistered<ISelectChecksToRecordPage>()
                ? _objectContainer.Resolve<ISelectChecksToRecordPage>()
                : null;

        public SelectChecksToRecordSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Select which checks to record page should be displayed")]
        public void ThenTheSelectWhichChecksToRecordPageShouldBeDisplayed()
        {
            Assert.True(
                selectChecksToRecordPage?.IsPageLoaded(),
                "Select which checks to record page was not loaded.");
        }

        [Then("there are {string} still to do")]
        public void ThenThereAreStillToDo(string checkType)
        {
            Assert.True(
                selectChecksToRecordPage?.IsCheckStillToDo(checkType),
                $"Expected '{checkType}' to be listed in the checks still to do.");
        }

        [When("the user ticks all 3 checkboxes")]
        public void WhenTheUserTicksAll3Checkboxes()
        {
            selectChecksToRecordPage?.TickAllCheckboxes();
        }

        [When("the user clicks Continue on the Select which checks to record page")]
        public void WhenTheUserClicksContinueOnTheSelectWhichChecksToRecordPage()
        {
            selectChecksToRecordPage?.ClickContinue();
        }
    }
}