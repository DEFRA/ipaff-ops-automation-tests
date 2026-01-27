using Reqnroll.BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Pages.Classes;
using System.ComponentModel;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class SealNumbersSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ISealNumbersPage? sealNumbersPage => _objectContainer.IsRegistered<ISealNumbersPage>() ? _objectContainer.Resolve<ISealNumbersPage>() : null;


        public SealNumbersSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Seal numbers page should be displayed")]
        public void ThenTheSealNumbersPageShouldBeDisplayed()
        {
            Assert.True(sealNumbersPage?.IsPageLoaded(), "Seal numbers page is not displayed");
        }

        [Then("{string} is pre-selected for Are new seal numbers required?")]
        public void ThenIsPreSelectedForAreNewSealNumbersRequired(string expectedSelection)
        {
            if (expectedSelection.Equals("No", StringComparison.OrdinalIgnoreCase))
            {
                _scenarioContext["AreNewSealNumbersRequired"] = expectedSelection;
                Assert.True(sealNumbersPage?.IsSealNumbersNoPreselected(), "No is not pre-selected for Are new seal numbers required?");
            }
        }

        [When("the user select {string} radio button on the Seal numbers page")]
        public void WhenISelectRadioButtonOnTheSealNumbersPage(string sealNum)
        {
            sealNumbersPage?.SelectSealNumRadio(sealNum);
            _scenarioContext["AreNewSealNumbersRequired"] = sealNum;
        }

        [When("the user enters new seal number {string}")]
        public void WhenTheUserEntersNewSealNumber(string sealNumber)
        {
            sealNumbersPage?.EnterNewSealNumber(sealNumber);
            _scenarioContext["NewSealNumber"] = sealNumber;
        }
    }
}