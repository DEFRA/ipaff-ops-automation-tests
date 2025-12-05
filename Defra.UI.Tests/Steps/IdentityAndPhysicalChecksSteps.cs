using Reqnroll.BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Pages.Classes;
using System.ComponentModel;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class IdentityAndPhysicalChecksSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IIdentityAndPhysicalChecksPage? identityAndPhysicalChecksPage => _objectContainer.IsRegistered<IIdentityAndPhysicalChecksPage>() ? _objectContainer.Resolve<IIdentityAndPhysicalChecksPage>() : null;


        public IdentityAndPhysicalChecksSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Identity and physical checks page should be displayed")]
        public void ThenTheIdentityAndPhysicalChecksPageShouldBeDisplayed()
        {
            Assert.True(identityAndPhysicalChecksPage?.IsPageLoaded());
        }

        [When("the user selects {string} under {string} in identity check")]
        public void WhenTheUserSelectsUnderInIdentityCheck(string decision, string checkType)
        {
            _scenarioContext.Add("IdentityCheckType", checkType);
            _scenarioContext.Add("IdentityCheckDecision", decision);
            identityAndPhysicalChecksPage?.ClickIdentityCheckOption(decision, checkType);
        }

        [When("the user selects {string} for physical check")]
        public void WhenTheUserSelectsForPhysicalCheck(string decision)
        {
            _scenarioContext.Add("PhysicalCheckDecision", decision);
            identityAndPhysicalChecksPage?.ClickPhysicalCheckDecision(decision);
        }
    }
}