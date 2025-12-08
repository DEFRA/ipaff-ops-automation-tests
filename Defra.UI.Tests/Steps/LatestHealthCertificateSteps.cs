using Reqnroll.BoDi;
using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class LatestHealthCertificateSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private ILatestHealthCertificatePage? latestHealthCertificatePage => _objectContainer.IsRegistered<ILatestHealthCertificatePage>() ? _objectContainer.Resolve<ILatestHealthCertificatePage>() : null;

        public LatestHealthCertificateSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Latest Health Certificate page should be displayed")]
        public void ThenTheLatestHealthCertificatePageShouldBeDisplayed()
        {
            Assert.True(latestHealthCertificatePage?.IsPageLoaded(), "Latest Health Certificate page not loaded");
        }

        [When("the user enters Latest Health Certificate Document reference {string}")]    
        public void WhenTheUserEntersDocumentReference(string reference)
        {
            latestHealthCertificatePage?.EnterDocumentReference(reference);
            _scenarioContext.Add("HealthCertificateReference", reference);
        }

        [When("the user enters Latest Health Certificate date of issue {string}{string}{string}")]    
        public void WhenTheUserEntersDateOfIssue(string day, string month, string year)
        {
            latestHealthCertificatePage?.EnterDateOfIssue(day, month, year);
            var dateofIssue = day + " " + month + " " + year;
            _scenarioContext.Add("HealthCertificateDateOfIssue", dateofIssue);
        }

        [When("the user adds Latest Health Certificate attachment")]
        public void WhenTheUserAddsLatestHealthCertificateAttachment()
        {
            throw new PendingStepException();
        }

    }
}