using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using DocumentFormat.OpenXml.Bibliography;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class AddCatchCertificateDetailsSteps
    {

        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IAddCatchCertificateDetailsPage? addCatchCertificateDetails => _objectContainer.IsRegistered<IAddCatchCertificateDetailsPage>() ? _objectContainer.Resolve<IAddCatchCertificateDetailsPage>() : null;


        public AddCatchCertificateDetailsSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("Add catch certificate details page should be displayed")]
        public void ThenAddCatchCertificatesDetailsPageShouldBeDisplayed()
        {
            Assert.True(addCatchCertificateDetails?.IsPageLoaded("Add catch certificate details"), "Add catch certificate details page not loaded");
        }

        [Then("{string} is displayed in Add catch certificate page")]
        public void ThenTheUserVerifiesThereAreCertificatesAttached(string text)
        {
            Assert.True(addCatchCertificateDetails?.VerifyContent(text), text+" is not displayed");
        }
        
        [When("the user enters {string} in catch certificate reference")]
        public void ThenTheUserEntersTheCatchCertificatesReference(string text)
        {
            addCatchCertificateDetails?.EnterCatchCertificateReference(text);
            Utils.AppendStringToScenarioContextArray(_scenarioContext, "CatchCertificateReference", text);           
        }
        
        [When("the user enters {string} in Flag state of catching vessels")]
        public void ThenTheUserEntersInFlagStateOfCatchingVessels(string state)
        {
            addCatchCertificateDetails?.EnterFlagStateOfCatchingVessel(state);
            Utils.AppendStringToScenarioContextArray(_scenarioContext, "FlagStateOfCatchingVessel", state);
        }
        
        [When("the user selects the {string} species under Select species being imported under this catch certificate")]
        public void WhenTheUserSelectAllUnderSelectSpecies(string species)
        {
            Thread.Sleep(2000);
            addCatchCertificateDetails?.SelectSpecies(species);
        }

        [When("the user enters Data of issue as {string}{string}{string} in Add catch certificate page")]
        public void ThenTheUserEntersDateOfIssue(string day, string month, string year)
        {
            addCatchCertificateDetails?.EnterDateOfIssue(day, month, year);

            var date = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
            var dateOfIssueCatchCertificate = date.ToString("d MMMM yyyy", CultureInfo.InvariantCulture);

            Utils.AppendStringToScenarioContextArray(_scenarioContext, "DateOfIssueCatchCertificate", dateOfIssueCatchCertificate);

        }

        [Then("the calendar icon is displayed in Add catch certificate page")]
        public void ThenTheCalendarIconIsDisplayedInAddCatchCertificatePage()
        {
            Assert.True(addCatchCertificateDetails?.VerifyCalendar(), "Calendar icon is not displayed");
        }
        
    }
}
