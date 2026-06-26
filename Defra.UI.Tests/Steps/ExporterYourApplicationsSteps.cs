using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterYourApplicationsSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IExporterYourApplicationsPage? exporterYourApplicationsPage =>
            _objectContainer.IsRegistered<IExporterYourApplicationsPage>()
                ? _objectContainer.Resolve<IExporterYourApplicationsPage>()
                : null;

        public ExporterYourApplicationsSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Exporter Portal homepage should be displayed")]
        [Then("the Your applications page is displayed")]
        public void ThenTheYourApplicationsPageIsDisplayed()
        {
            Assert.True(exporterYourApplicationsPage?.IsPageLoaded(), "Your applications page is not displayed");
        }

        [When("the user clicks the Start a new application button")]
        public void WhenTheUserClicksTheStartANewApplicationButton()
        {
            exporterYourApplicationsPage?.ClickStartNewApplicationButton();
        }
    }
}