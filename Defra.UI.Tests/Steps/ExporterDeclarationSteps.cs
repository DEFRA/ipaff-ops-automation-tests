using Defra.UI.Tests.Pages.Interfaces;
using Reqnroll;
using Reqnroll.BoDi;
using NUnit.Framework;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterDeclarationSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IExporterDeclarationPage? exporterDeclarationPage =>
            _objectContainer.IsRegistered<IExporterDeclarationPage>()
                ? _objectContainer.Resolve<IExporterDeclarationPage>()
                : null;

        public ExporterDeclarationSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Exporter declaration page is displayed")]
        public void ThenTheExporterDeclarationPageIsDisplayed()
        {
            Assert.True(exporterDeclarationPage?.IsPageLoaded(), "Exporter declaration page is not displayed");
        }

        [When("the user ticks the exporter declaration checkbox")]
        public void WhenTheUserTicksTheExporterDeclarationCheckbox()
        {
            exporterDeclarationPage?.TickDeclarationCheckbox();
        }

        [When("the user clicks the Submit application button")]
        public void WhenTheUserClicksTheSubmitApplicationButton()
        {
            exporterDeclarationPage?.ClickSubmitApplicationButton();
        }
    }
}