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
            Assert.True(identityAndPhysicalChecksPage?.IsPageLoaded(), "Identity and physical checks page is not displayed");
        }

        [Then("the Identity, physical and welfare checks page should be displayed")]
        public void ThenTheIdentityPhysicalAndWelfareChecksPageShouldBeDisplayed()
        {
            Assert.True(identityAndPhysicalChecksPage?.IsIdentityPhysicalAndWelfareChecksPageLoaded(), "Identity, physical and welfare checks page is not displayed");
        }

        [When("the user selects {string} under {string} in identity check")]
        public void WhenTheUserSelectsUnderInIdentityCheck(string decision, string checkType)
        {
            _scenarioContext.Add("IdentityCheckType", checkType);
            _scenarioContext.Add("IdentityCheckDecision", decision);
            identityAndPhysicalChecksPage?.ClickIdentityCheckOption(decision, checkType);
        }

        [When("the user selects {string} for Physical check")]
        [When("the user selects {string} for physical check")]
        public void WhenTheUserSelectsForPhysicalCheck(string decision)
        {
            _scenarioContext.Add("PhysicalCheckDecision", decision);
            identityAndPhysicalChecksPage?.SelectPhysicalCheck(decision);
        }

        [When("the user selects {string} for Identity check")]
        public void WhenTheUserSelectsForIdentityCheck(string decision)
        {
            _scenarioContext.Add("IdentityCheckDecision", decision);
            identityAndPhysicalChecksPage?.SelectIdentityCheck(decision);
        }

        [When("the user selects {string} for Number of animals checked")]
        public void WhenTheUserSelectsForNumberOfAnimalsChecked(string numberOfAnimals)
        {
            if (!string.IsNullOrEmpty(numberOfAnimals))
            {
                _scenarioContext.Add("NumberOfAnimalsChecked", numberOfAnimals);
            }
            identityAndPhysicalChecksPage?.EnterNumberOfAnimalsChecked(numberOfAnimals);
        }

        [When("the user selects {string} for Welfare check")]
        public void WhenTheUserSelectsForWelfareCheck(string decision)
        {
            if (!string.IsNullOrEmpty(decision))
            {
                _scenarioContext.Add("WelfareCheckDecision", decision);
            }
            identityAndPhysicalChecksPage?.SelectWelfareCheck(decision);
        }

        [When("the user selects {string} {string} for Number of dead animals")]
        public void WhenTheUserSelectsForNumberOfDeadAnimals(string numberOfDeadAnimals, string unit)
        {
            if (!string.IsNullOrEmpty(numberOfDeadAnimals))
            {
                _scenarioContext.Add("NumberOfDeadAnimals", numberOfDeadAnimals);
            }
            if (!string.IsNullOrEmpty(unit))
            {
                _scenarioContext.Add("NumberOfDeadAnimalsUnit", unit);
            }
            identityAndPhysicalChecksPage?.EnterNumberOfDeadAnimals(numberOfDeadAnimals, unit);
        }

        [When("the user selects {string} {string} for Number of unfit animals")]
        public void WhenTheUserSelectsForNumberOfUnfitAnimals(string numberOfUnfitAnimals, string unit)
        {
            if (!string.IsNullOrEmpty(numberOfUnfitAnimals))
            {
                _scenarioContext.Add("NumberOfUnfitAnimals", numberOfUnfitAnimals);
            }
            if (!string.IsNullOrEmpty(unit))
            {
                _scenarioContext.Add("NumberOfUnfitAnimalsUnit", unit);
            }
            identityAndPhysicalChecksPage?.EnterNumberOfUnfitAnimals(numberOfUnfitAnimals, unit);
        }

        [When("the user selects {string} for Number of births or abortions")]
        public void WhenTheUserSelectsForNumberOfBirthsOrAbortions(string numberOfBirths)
        {
            if (!string.IsNullOrEmpty(numberOfBirths))
            {
                _scenarioContext.Add("NumberOfBirthsOrAbortions", numberOfBirths);
            }
            identityAndPhysicalChecksPage?.EnterNumberOfBirthsOrAbortions(numberOfBirths);
        }
    }
}