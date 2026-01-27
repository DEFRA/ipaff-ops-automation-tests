using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
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
            _scenarioContext["IdentityCheckType"] = checkType;
            _scenarioContext["IdentityCheckDecision"] = decision;
            identityAndPhysicalChecksPage?.ClickIdentityCheckOption(decision, checkType);
        }

        [When("the user selects {string} for Physical check")]
        [When("the user selects {string} for physical check")]
        public void WhenTheUserSelectsForPhysicalCheck(string decision)
        {
            _scenarioContext["PhysicalCheckDecision"] = decision;
            identityAndPhysicalChecksPage?.SelectPhysicalCheck(decision);
        }

        [When("the user selects {string} for Identity check")]
        public void WhenTheUserSelectsForIdentityCheck(string decision)
        {
            _scenarioContext["IdentityCheckDecision"] = decision;
            identityAndPhysicalChecksPage?.SelectIdentityCheck(decision);
        }

        [When("the user selects {string} for Number of animals checked")]
        public void WhenTheUserSelectsForNumberOfAnimalsChecked(string numberOfAnimals)
        {
            if (!string.IsNullOrEmpty(numberOfAnimals))
            {
                _scenarioContext["NumberOfAnimalsChecked"] = numberOfAnimals;
            }
            identityAndPhysicalChecksPage?.EnterNumberOfAnimalsChecked(numberOfAnimals);
        }

        [When("the user selects {string} for Welfare check")]
        public void WhenTheUserSelectsForWelfareCheck(string decision)
        {
            if (!string.IsNullOrEmpty(decision))
            {
                _scenarioContext["WelfareCheckDecision"] = decision;
            }
            identityAndPhysicalChecksPage?.SelectWelfareCheck(decision);
        }

        [When("the user selects {string} {string} for Number of dead animals")]
        public void WhenTheUserSelectsForNumberOfDeadAnimals(string numberOfDeadAnimals, string unit)
        {
            if (!string.IsNullOrEmpty(numberOfDeadAnimals))
            {
                _scenarioContext["NumberOfDeadAnimals"] = numberOfDeadAnimals;
            }
            if (!string.IsNullOrEmpty(unit))
            {
                _scenarioContext["NumberOfDeadAnimalsUnit"] = unit;
            }
            identityAndPhysicalChecksPage?.EnterNumberOfDeadAnimals(numberOfDeadAnimals, unit);
        }

        [When("the user selects {string} {string} for Number of unfit animals")]
        public void WhenTheUserSelectsForNumberOfUnfitAnimals(string numberOfUnfitAnimals, string unit)
        {
            if (!string.IsNullOrEmpty(numberOfUnfitAnimals))
            {
                _scenarioContext["NumberOfUnfitAnimals"]= numberOfUnfitAnimals;
            }
            if (!string.IsNullOrEmpty(unit))
            {
                _scenarioContext["NumberOfUnfitAnimalsUnit"] = unit;
            }
            identityAndPhysicalChecksPage?.EnterNumberOfUnfitAnimals(numberOfUnfitAnimals, unit);
        }

        [When("the user selects {string} for Number of births or abortions")]
        public void WhenTheUserSelectsForNumberOfBirthsOrAbortions(string numberOfBirths)
        {
            if (!string.IsNullOrEmpty(numberOfBirths))
            {
                _scenarioContext["NumberOfBirthsOrAbortions"] = numberOfBirths;
            }
            identityAndPhysicalChecksPage?.EnterNumberOfBirthsOrAbortions(numberOfBirths);
        }

        [When("the user clicks Save and Return")]
        public void WhenTheUserClicksSaveAndReturn()
        {
            identityAndPhysicalChecksPage?.ClickSaveAndReturn();
        }
    }
}