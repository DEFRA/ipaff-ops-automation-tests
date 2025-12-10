using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class HealthCertificateSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IHealthCertificatePage? healthCertificatePage => _objectContainer.IsRegistered<IHealthCertificatePage>() ? _objectContainer.Resolve<IHealthCertificatePage>() : null;

        public HealthCertificateSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Health certificate required page should be displayed")]
        public void ThenTheHealthCertificateRequiredPageShouldBeDisplayed()
        {
            Assert.True(healthCertificatePage?.IsPageLoaded(), "Health certificate required page not loaded");
        }


        [When("the user clicks continue button")]
        public void WhenTheUserClicksContinueButton()
        {
            healthCertificatePage?.ClickContinue();
        }
    }
}