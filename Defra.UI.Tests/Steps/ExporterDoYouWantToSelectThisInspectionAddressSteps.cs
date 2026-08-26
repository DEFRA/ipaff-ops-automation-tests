using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterDoYouWantToSelectThisInspectionAddressSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IExporterDoYouWantToSelectThisInspectionAddressPage? exporterDoYouWantToSelectThisInspectionAddressPage =>
            _objectContainer.IsRegistered<IExporterDoYouWantToSelectThisInspectionAddressPage>()
                ? _objectContainer.Resolve<IExporterDoYouWantToSelectThisInspectionAddressPage>()
                : null;

        public ExporterDoYouWantToSelectThisInspectionAddressSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Do you want to select this inspection address? page is displayed")]
        public void ThenTheDoYouWantToSelectThisInspectionAddressPageIsDisplayed()
        {
            Assert.True(exporterDoYouWantToSelectThisInspectionAddressPage?.IsPageLoaded(), "Do you want to select this inspection address? page is not displayed");
        }

        [When("the user selects {string} and clicks the Continue button on the Do you want to select this inspection address? page")]
        public void WhenTheUserSelectsAndClicksTheContinueButtonOnTheDoYouWantToSelectThisInspectionAddressPage(string option)
        {
            exporterDoYouWantToSelectThisInspectionAddressPage?.SelectInspectionAddressOptionAndContinue(option);
        }
    }
}