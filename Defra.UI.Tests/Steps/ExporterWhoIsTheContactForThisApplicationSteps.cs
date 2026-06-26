using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterWhoIsTheContactForThisApplicationSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IExporterWhoIsTheContactForThisApplicationPage? exporterWhoIsTheContactForThisApplicationPage =>
            _objectContainer.IsRegistered<IExporterWhoIsTheContactForThisApplicationPage>()
                ? _objectContainer.Resolve<IExporterWhoIsTheContactForThisApplicationPage>()
                : null;

        public ExporterWhoIsTheContactForThisApplicationSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Who is the contact for this application? page is displayed")]
        public void ThenTheWhoIsTheContactForThisApplicationPageIsDisplayed()
        {
            Assert.True(exporterWhoIsTheContactForThisApplicationPage?.IsPageLoaded(), "Who is the contact for this application? page is not displayed");
        }

        [When("the user enters valid contact details and clicks the Save and continue button")]
        public void WhenTheUserEntersValidContactDetailsAndClicksTheSaveAndContinueButton()
        {
            exporterWhoIsTheContactForThisApplicationPage?.EnterContactDetails(
                "Automation Org",
                "Automation User",
                Utils.GenerateRandomUKPhonenumber());

            exporterWhoIsTheContactForThisApplicationPage?.ClickSaveAndContinueButton();
        }
    }
}