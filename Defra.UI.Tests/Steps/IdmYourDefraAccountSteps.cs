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
        private IIdmYourDefraAccountPage page => _objectContainer.Resolve<IIdmYourDefraAccountPage>();
        public IdmYourDefraAccountSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the Your Defra account page shows the registered details and organisation Admin role")]
        public void ThenAccountDetailsAreCorrect()
        {
            var business = _scenarioContext.Get<OperatorDetails>("RegistrationBusiness");
            var fullName = $"{_scenarioContext.Get<string>("RegistrationFirstName")} {_scenarioContext.Get<string>("RegistrationLastName")}";
            Assert.That(page.IsPageLoaded(), Is.True);
            Assert.Multiple(() =>
            {
                Assert.That(page.GetFullName(), Is.EqualTo(fullName));
                Assert.That(page.GetEmail(), Is.EqualTo(_scenarioContext.Get<string>("RegistrationEmail")));
                Assert.That(page.GetPhone(), Is.EqualTo(_scenarioContext.Get<string>("RegistrationPhone")));
                Assert.That(page.HasExporterAdminAccount(business.OperatorName), Is.True, "Expected the registered organisation with Admin and Plant Health Exporter roles");
            });
        }

        [When("the user closes the Defra account window and returns to the exporter")]
        public void WhenAccountWindowIsClosed() => page.CloseAndReturnToExporter(_scenarioContext.Get<string>("IdmExporterWindow"));
    }
}
