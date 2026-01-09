using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class RecordLaboratoryTestInformationSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IRecordLaboratoryTestInformationPage? recordLaboratoryTestInformation => _objectContainer.IsRegistered<IRecordLaboratoryTestInformationPage>() ? _objectContainer.Resolve<IRecordLaboratoryTestInformationPage>() : null;

        public RecordLaboratoryTestInformationSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Record laboratory test information page should be displayed")]
        public void ThenTheRecordlabtestinfoPageShouldBeDisplayed()
        {
            Assert.True(recordLaboratoryTestInformation?.IsPageLoaded(), "Record laboratory test information page not loaded");
        }

        [When("the user selects {string} for Conclusion")]
        public void WhenTheUserSelectsForConclusion(string decision)
        {
            _scenarioContext["Conclusion"] = decision;
            recordLaboratoryTestInformation?.SelectConclusion(decision);
        }

        [When("the user enters Sample use by date as {string}{string}{string}")]
        public void WhenTheUserEntersSampleUseByDate(string day, string month, string year)
        {
            recordLaboratoryTestInformation?.EnterSampleUseByDate(day, month, year);

            var date = DateTime.ParseExact($"{day}/{month}/{year}", "d/M/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None);

            _scenarioContext["SampleUseByDate"] = date.ToString("d MMMM yyyy");
        }

        [When("the user enters Released date as {string}{string}{string}")]
        public void WhenTheUserEntersReleasedDate(string day, string month, string year)
        {
            recordLaboratoryTestInformation?.EnterReleasedDate(day, month, year);

            var date = DateTime.ParseExact($"{day}/{month}/{year}", "d/M/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None);

            _scenarioContext["ReleasedDate"] = date.ToString("d MMMM yyyy");
        }
    }
}