using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterHowDoYouWantToAddCommoditiesToYourConsignmentSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IExporterHowDoYouWantToAddCommoditiesToYourConsignmentPage? exporterHowDoYouWantToAddCommoditiesToYourConsignmentPage =>
            _objectContainer.IsRegistered<IExporterHowDoYouWantToAddCommoditiesToYourConsignmentPage>()
                ? _objectContainer.Resolve<IExporterHowDoYouWantToAddCommoditiesToYourConsignmentPage>()
                : null;

        public ExporterHowDoYouWantToAddCommoditiesToYourConsignmentSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the How do you want to add commodities to your consignment? page is displayed")]
        public void ThenTheHowDoYouWantToAddCommoditiesToYourConsignmentPageIsDisplayed()
        {
            Assert.True(exporterHowDoYouWantToAddCommoditiesToYourConsignmentPage?.IsPageLoaded(), "How do you want to add commodities to your consignment? page is not displayed");
        }

        [When("the user selects {string} and clicks the Continue button on the How do you want to add commodities to your consignment? page")]
        public void WhenTheUserSelectsAndClicksTheContinueButtonOnTheHowDoYouWantToAddCommoditiesToYourConsignmentPage(string addMethod)
        {
            exporterHowDoYouWantToAddCommoditiesToYourConsignmentPage?.SelectCommodityAddMethod(addMethod);
            exporterHowDoYouWantToAddCommoditiesToYourConsignmentPage?.ClickContinueButton();
        }
    }
}