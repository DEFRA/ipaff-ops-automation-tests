using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmYourApplicationsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private IExporterYourApplicationsPage page => _objectContainer.Resolve<IExporterYourApplicationsPage>();
        public IdmYourApplicationsSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the newly registered exporter can start a new application")]
        public void ThenNewApplicationIsAvailable() => Assert.That(page.IsStartNewApplicationAvailable(), Is.True);

        [When("the user clicks Manage account on the exporter dashboard")]
        public void WhenManageAccountIsClicked() => page.ClickManageAccountLink();

        [When("the user signs out of the exporter dashboard")]
        public void WhenSignOutIsClicked() => page.ClickSignOutLink();
    }
}
