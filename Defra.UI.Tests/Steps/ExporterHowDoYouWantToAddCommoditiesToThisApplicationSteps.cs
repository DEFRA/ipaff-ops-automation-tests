using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterHowDoYouWantToAddCommoditiesToThisApplicationSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IExporterHowDoYouWantToAddCommoditiesToThisApplicationPage? exporterHowDoYouWantToAddCommoditiesToThisApplicationPage =>
            _objectContainer.IsRegistered<IExporterHowDoYouWantToAddCommoditiesToThisApplicationPage>()
                ? _objectContainer.Resolve<IExporterHowDoYouWantToAddCommoditiesToThisApplicationPage>()
                : null;

        public ExporterHowDoYouWantToAddCommoditiesToThisApplicationSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the How do you want to add commodities to this application? page is displayed")]
        public void ThenTheHowDoYouWantToAddCommoditiesToThisApplicationPageIsDisplayed()
        {
            Assert.True(exporterHowDoYouWantToAddCommoditiesToThisApplicationPage?.IsPageLoaded(), "How do you want to add commodities to this application? page is not displayed");
        }

        [When("the user selects {string} and clicks the Continue button on the How do you want to add commodities to this application? page")]
        public void WhenTheUserSelectsAndClicksTheContinueButtonOnTheHowDoYouWantToAddCommoditiesToThisApplicationPage(string option)
        {
            exporterHowDoYouWantToAddCommoditiesToThisApplicationPage?.SelectCommodityAddMethod(option);
            exporterHowDoYouWantToAddCommoditiesToThisApplicationPage?.ClickContinueButton();
        }
    }
}
