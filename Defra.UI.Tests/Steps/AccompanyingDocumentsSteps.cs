using Reqnroll.BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.CP
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

        [When("the user enters date of issue {string}{string}{string}")]
        public void WhenTheUserEntersDateOfIssue(string day, string month, string year)
        {
            accompanyingDocumentsPage?.EnterDateOfIssue(day, month, year);
            string monthName = new DateTime(2000, int.Parse(month), 1).ToString("MMMM");
            var dateofIssue = day + " " + monthName + " " + year;
             _scenarioContext.Add("DateOfIssue", dateofIssue);
        }
    }
}