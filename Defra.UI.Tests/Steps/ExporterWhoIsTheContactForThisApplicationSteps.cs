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

        [When("the user enters a contact name for the inspector to contact about the application")]
        public void WhenTheUserEntersAContactNameForTheInspectorToContactAboutTheApplication()
        {
            exporterWhoIsTheContactForThisApplicationPage?.EnterContactName("Automation User");
        }

        [When("the user enters a phone number for the inspector to contact about the application")]
        public void WhenTheUserEntersAPhoneNumberForTheInspectorToContactAboutTheApplication()
        {
            exporterWhoIsTheContactForThisApplicationPage?.EnterPhoneNumber(Utils.GenerateRandomUKPhonenumber());
        }

        [Then("the fields available on the Who is the contact for this application? page are {string}")]
        public void ThenTheFieldsAvailableOnTheWhoIsTheContactForThisApplicationPageAre(string fields)
        {
            var expected = fields.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
            var actual = exporterWhoIsTheContactForThisApplicationPage?.GetContactFieldLabels();
            Assert.That(actual, Is.EqualTo(expected), $"Contact fields were '{string.Join(", ", actual ?? new List<string>())}'");
        }

        [When("the user enters an email address for the inspector to contact about the application")]
        public void WhenTheUserEntersAnEmailAddressForTheInspectorToContactAboutTheApplication()
        {
            exporterWhoIsTheContactForThisApplicationPage?.EnterEmailAddress("automation.user@example.com");
        }

        [When("the user clicks the Save and continue button on the Who is the contact for this application? page")]
        public void WhenTheUserClicksTheSaveAndContinueButtonOnTheWhoIsTheContactForThisApplicationPage()
        {
            exporterWhoIsTheContactForThisApplicationPage?.ClickSaveAndContinueButton();
        }
    }
}