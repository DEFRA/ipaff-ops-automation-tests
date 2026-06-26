using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterWhenAndWhereWillTheConsignmentBeReadySteps
    {
        private readonly IObjectContainer _objectContainer;

        private IExporterWhenAndWhereWillTheConsignmentBeReadyPage? exporterWhenAndWhereWillTheConsignmentBeReadyPage =>
            _objectContainer.IsRegistered<IExporterWhenAndWhereWillTheConsignmentBeReadyPage>()
                ? _objectContainer.Resolve<IExporterWhenAndWhereWillTheConsignmentBeReadyPage>()
                : null;

        public ExporterWhenAndWhereWillTheConsignmentBeReadySteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the When and where will the consignment be ready? page is displayed")]
        public void ThenTheWhenAndWhereWillTheConsignmentBeReadyPageIsDisplayed()
        {
            Assert.True(exporterWhenAndWhereWillTheConsignmentBeReadyPage?.IsPageLoaded(), "When and where will the consignment be ready? page is not displayed");
        }

        [When("the user enters a valid ready date, time and place and clicks the Save and continue button")]
        public void WhenTheUserEntersAValidReadyDateTimeAndPlaceAndClicksTheSaveAndContinueButton()
        {
            exporterWhenAndWhereWillTheConsignmentBeReadyPage?.EnterReadyDateTimeAndPlace(DateTime.Today.AddDays(2), "10:30","am", "Main Warehouse Gate");
            exporterWhenAndWhereWillTheConsignmentBeReadyPage?.ClickSaveAndContinueButton();
        }
    }
}