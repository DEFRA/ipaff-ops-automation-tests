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
    public class AboutConsignmentSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private IAboutConsignmentPage? aboutConsignmentPage => _objectContainer.IsRegistered<IAboutConsignmentPage>() ? _objectContainer.Resolve<IAboutConsignmentPage>() : null;


        public AboutConsignmentSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the About the consignment\\/What are you importing? page should be displayed with radio buttons")]
        public void ThenTheAboutTheConsignmentWhatAreYouImportingPageShouldBeDisplayedWithRadioButtons()
        {
            Assert.True(aboutConsignmentPage?.IsPageLoaded(), "About the consignment What are you importing? page not loaded");
            Assert.True(aboutConsignmentPage?.AreImportOptionsPresent(), "Expected import options are not present on the page.");
        }

        [When("the user chooses {string} option")]
        public void WhenTheUserChoosesOption(string option)
        {
            aboutConsignmentPage?.ClickImportingProduct(option);
            _scenarioContext.Add("ImportType", option);
        }

        [Then("the user should be able to click Save and continue")]
        [When("the user clicks Save and continue")]
        [Then("the user clicks Save and continue")]
        public void WhenTheUserClicksSaveAndContinue()
        {
            aboutConsignmentPage?.ClickSaveAndContinue();
        }
    }
}