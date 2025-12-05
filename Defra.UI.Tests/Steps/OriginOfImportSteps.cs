using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using DocumentFormat.OpenXml.Drawing.Charts;
using Faker;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class OriginOfImportSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IOriginOfImportPage? originOfImportPage => _objectContainer.IsRegistered<IOriginOfImportPage>() ? _objectContainer.Resolve<IOriginOfImportPage>() : null;


        public OriginOfImportSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Origin of the import page should be displayed, showing {string} as the Country of origin and Country from where consigned")]
        public void ThenTheOriginOfTheImportPageShouldBeDisplayedShowingAsTheCountryOfOriginAndCountryFromWhereConsigned(string country)
        {
            Assert.True(originOfImportPage?.IsPageLoaded(), "About the consignment Origin of the import page not loaded");
            _scenarioContext.Add("ContryFromWhereConsigned", country);
        }

        [When("the user chooses {string} for Does your consignment require a region code?")]
        public void WhenTheUserChoosesForDoesYourConsignmentRequireARegionCode(string option)
        {
            originOfImportPage?.IsRegionOfOriginCodeNeeded(option);
            _scenarioContext.Add("IsRegionOfOriginCodeRequired", option);
        }

        [When("the user chooses {string} for Does this consignment conform to regulatory regulations?")]
        public void WhenTheUserChoosesForDoesThisConsignmentConformToRegulatoryRegulations(string option)
        {
            originOfImportPage?.IsConformToRegulatoryRequirements(option);
            _scenarioContext.Add("ConsignmentConformToRegulatoryRequirements", option);
        }

        [When("the user chooses {string} for Will the consignment change vehicles or means of transport after the Border Control Post \\(BCP)?")]
        public void WhenTheUserChoosesForWillTheConsignmentChangeVehiclesOrMeansOfTransportAfterTheBorderControlPostBCP(string option)
        {
            originOfImportPage?.IsItAfterBCP(option);
        }

        [When("the user enters a reference number {string} in the Add a reference number for this consignment \\(optional) field")]
        public void WhenTheUserEntersAReferenceNumberInTheAddAReferenceNumberForThisConsignmentOptionalField(string refNum)
        {
            originOfImportPage?.EnterConsignmentRefNum(refNum);
            _scenarioContext.Add("ConsignmentReferenceNumber", refNum);
        }
    }
}