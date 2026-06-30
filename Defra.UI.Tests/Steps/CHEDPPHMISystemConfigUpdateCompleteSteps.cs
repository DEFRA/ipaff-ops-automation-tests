using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class CHEDPPHMISystemConfigUpdateCompleteSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ICHEDPPHMISystemConfigUpdateCompletePage? chedPPHMISystemConfigUpdateCompletePage =>
            _objectContainer.IsRegistered<ICHEDPPHMISystemConfigUpdateCompletePage>()
                ? _objectContainer.Resolve<ICHEDPPHMISystemConfigUpdateCompletePage>()
                : null;

        public CHEDPPHMISystemConfigUpdateCompleteSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the System configuration update page should be displayed")]
        public void ThenTheSystemConfigurationUpdatePageShouldBeDisplayed()
        {
            Assert.True(chedPPHMISystemConfigUpdateCompletePage?.IsPageLoaded(), "System Configuration Update Complete page is not displayed");
        }

        [Then("the system configuration update successful message should be displayed")]
        public void ThenTheSystemConfigurationUpdateSuccessfulMessageShouldBeDisplayedDiplayed()
        {
            Assert.True(chedPPHMISystemConfigUpdateCompletePage?.VerifySystemConfigCompleteText(), "System Configuration Update Complete page is not displayed");
        }
    }
}