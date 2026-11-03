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

        [Then("the Your Defra account page shows the registered details and organisation Admin role")]
        public void ThenAccountDetailsAreCorrect()
        {
            var businessName = _scenarioContext.Get<string>("RegistrationBusinessName");
            var fullName = $"{_scenarioContext.Get<string>("RegistrationFirstName")} {_scenarioContext.Get<string>("RegistrationLastName")}";
            Assert.That(idmYourDefraAccountPage.IsPageLoaded(), Is.True);
            Assert.Multiple(() =>
            {
                Assert.That(idmYourDefraAccountPage.GetFullName(), Is.EqualTo(fullName));
                Assert.That(idmYourDefraAccountPage.GetEmail(), Is.EqualTo(_scenarioContext.Get<string>("RegistrationEmail")));
                Assert.That(idmYourDefraAccountPage.GetPhone(), Is.EqualTo(_scenarioContext.Get<string>("RegistrationPhone")));
                Assert.That(idmYourDefraAccountPage.HasExporterAdminAccount(businessName), Is.True, "Expected the registered organisation with Admin and Plant Health Exporter roles");
            });
        }

        [When("the user closes the Defra account window and returns to the exporter")]
        public void WhenAccountWindowIsClosed() => idmYourDefraAccountPage.CloseAndReturnToExporter(_scenarioContext.Get<string>("IdmExporterWindow"));
    }
}
