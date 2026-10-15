using Defra.UI.Tests.Pages.Interfaces;
using DocumentFormat.OpenXml.Bibliography;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterWhenWillTheConsignmentBeReadySteps
    {
        private readonly IObjectContainer _objectContainer;

        private IExporterWhenWillTheConsignmentBeReadyPage? exporterWhenWillTheConsignmentBeReadyPage =>
            _objectContainer.IsRegistered<IExporterWhenWillTheConsignmentBeReadyPage>()
                ? _objectContainer.Resolve<IExporterWhenWillTheConsignmentBeReadyPage>()
                : null;

        public ExporterWhenWillTheConsignmentBeReadySteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the When will the consignment be ready? page is displayed")]
        public void ThenTheWhenWillTheConsignmentBeReadyPageIsDisplayed()
        {
            Assert.True(exporterWhenWillTheConsignmentBeReadyPage?.IsPageLoaded(), "When will the consignment be ready? page is not displayed");
        }

        [When("the user enters a valid date for when the consignment will be ready for inspection at least {int} days in the future")]
        public void WhenTheUserEntersAValidDateForWhenTheConsignmentWillBeReadyForInspectionAtLeastDaysInTheFuture(int days)
        {
            exporterWhenWillTheConsignmentBeReadyPage?.EnterReadyDate(DateTime.Today.AddDays(days));
        }

        [When("the user clicks the Save and continue button on the When will the consignment be ready? page")]
        public void WhenTheUserClicksTheSaveAndContinueButtonOnTheWhenWillTheConsignmentBeReadyPage()
        {
            exporterWhenWillTheConsignmentBeReadyPage?.ClickSaveAndContinueButton();
        }
    }
}
