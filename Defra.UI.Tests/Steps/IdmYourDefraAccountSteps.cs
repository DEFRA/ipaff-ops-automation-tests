using Defra.UI.Tests.Tools;
using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmYourDefraAccountSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private IIdmYourDefraAccountPage idmYourDefraAccountPage => _objectContainer.Resolve<IIdmYourDefraAccountPage>();
        public IdmYourDefraAccountSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the Your Defra account page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(idmYourDefraAccountPage.IsPageLoaded(), Is.True, "Your Defra account page is not displayed");

        [Then("the details on the Your Defra account page match the registered details")]
        public void ThenAccountDetailsAreCorrect()
        {
            var businessName = _scenarioContext.Get<string>("RegistrationBusinessName");
            var fullName = $"{_scenarioContext.Get<string>("RegistrationFirstName")} {_scenarioContext.Get<string>("RegistrationLastName")}";
            Assert.Multiple(() =>
            {
                Assert.That(idmYourDefraAccountPage.GetFullName(), Is.EqualTo(fullName));
                Assert.That(idmYourDefraAccountPage.GetEmail(), Is.EqualTo(_scenarioContext.Get<string>("RegistrationEmail")));
                Assert.That(idmYourDefraAccountPage.GetPhone(), Is.EqualTo(_scenarioContext.Get<string>("RegistrationPhone")));
                Assert.That(idmYourDefraAccountPage.HasExporterAdminAccount(businessName), Is.True, "Expected the registered organisation with Admin and Plant Health Exporter roles");
            });
        }

        [When("the user closes the Your Defra account window")]
        public void WhenAccountWindowIsClosed() => idmYourDefraAccountPage.CloseAndReturnToExporter(_scenarioContext.Get<string>("IdmExporterWindow"));
    }
}
