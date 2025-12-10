using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;
using System.Globalization;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class AccompanyingDocumentsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IAccompanyingDocumentsPage? accompanyingDocumentsPage => _objectContainer.IsRegistered<IAccompanyingDocumentsPage>() ? _objectContainer.Resolve<IAccompanyingDocumentsPage>() : null;

        public AccompanyingDocumentsSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Accompanying documents page should be displayed")]
        public void ThenTheAccompanyingDocumentsPageShouldBeDisplayed()
        {
            Assert.True(accompanyingDocumentsPage?.IsPageLoaded(), "Accompanying documents");
        }


        [When("the user selects Document type {string}")]
        public void WhenTheUserSelectsDocumentType(string type)
        {
            accompanyingDocumentsPage?.SelectDocumentType(type);
            _scenarioContext.Add("DocumentType", type);
        }

        [When("the user enters Document reference {string}")]
        public void WhenTheUserEntersDocumentReference(string reference)
        {
            accompanyingDocumentsPage?.EnterDocumentReference(reference);
            _scenarioContext.Add("DocumentReference", reference);
        }

        [When("the user enters date of issue {string}")]
        public void WhenTheUserEntersDateOfIssue(string dateString)
        {
            var date = Utils.ConvertToDate(dateString);
            accompanyingDocumentsPage?.EnterDateOfIssue(date.Day.ToString(), date.Month.ToString(), date.Year.ToString());
            var monthName = date.ToString("MMMM", CultureInfo.InvariantCulture);
            var dateofIssue = date.Day.ToString() + " " + monthName + " " + date.Year.ToString();
            _scenarioContext.Add("DateOfIssue", dateofIssue);
        }

        [When("the user enters date of issue {string}{string}{string}")]
        public void WhenTheUserEntersDateOfIssue(string day, string month, string year)
        {
            accompanyingDocumentsPage?.EnterDateOfIssue(day, month, year);
            var dateofIssue = day + " " + month + " " + year;
            _scenarioContext.Add("DocumentDateOfIssue", dateofIssue);
        }
    }
}