using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterWhoAreYouSendingThisConsignmentToSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IExporterWhoAreYouSendingThisConsignmentToPage? exporterWhoAreYouSendingThisConsignmentToPage =>
            _objectContainer.IsRegistered<IExporterWhoAreYouSendingThisConsignmentToPage>()
                ? _objectContainer.Resolve<IExporterWhoAreYouSendingThisConsignmentToPage>()
                : null;

        public ExporterWhoAreYouSendingThisConsignmentToSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Who are you sending this consignment to? page is displayed")]
        public void ThenTheWhoAreYouSendingThisConsignmentToPageIsDisplayed()
        {
            Assert.True(exporterWhoAreYouSendingThisConsignmentToPage?.IsPageLoaded(), "Who are you sending this consignment to? page is not displayed");
        }

        [When("the user enters the conisgnee details and clicks the Continue button on the Who are you sending this consignment to? page")]
        public void WhenTheUserEntersTheConisgneeDetailsAndClicksTheContinueButtonOnTheWhoAreYouSendingThisConsignmentToPage()
        {
            var consignee = Utils.GenerateOperatorDetails();

            exporterWhoAreYouSendingThisConsignmentToPage?.EnterConsigneeDetails(
                consignee.OperatorName,
                consignee.AddressLine1,
                string.Empty,
                consignee.CityOrTown,
                string.Empty,
                consignee.Postcode);

            exporterWhoAreYouSendingThisConsignmentToPage?.ClickSaveAndContinueButton();
        }
    }
}
