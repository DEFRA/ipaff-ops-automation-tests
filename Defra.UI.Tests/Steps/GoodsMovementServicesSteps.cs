using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class GoodsMovementServicesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IGoodsMovementServicesPage? goodsMovementServicesPage => _objectContainer.IsRegistered<IGoodsMovementServicesPage>() ? _objectContainer.Resolve<IGoodsMovementServicesPage>() : null;

        public GoodsMovementServicesSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Goods movement services page should be displayed")]
        public void ThenTheGoodsMovementServicesPageShouldBeDisplayed()
        {
            Assert.True(goodsMovementServicesPage?.IsPageLoaded(), "Transport Goods movement services page not loaded");
        }

        [When("the user selects {string} for Are you using the Common Transit Convention \\(CTC)?")]
        public void WhenTheUserSelectsForAreYouUsingTheCommonTransitConventionCTC(string option)
        {
            goodsMovementServicesPage?.CTCToMoveGoods(option);
            _scenarioContext["IsCTC"] = option;
        }

        [When("the user selects {string} for Will the transport use the Goods Vehicle Movement Service \\(GVMS)?")]
        public void WhenTheUserSelectsForWillTheTransportUseTheGoodsVehicleMovementServiceGVMS(string option)
        {
            goodsMovementServicesPage?.GVMSToMoveGoods(option);
            _scenarioContext["IsGVMS"] = option;
        }

        [Then("the user verifies and enters any missing data on the Goods movement services page")]
        public void ThenTheUserVerifiesAndEntersAnyMissingDataOnTheGoodsMovementServicesPage()
        {
            WhenTheUserSelectsForAreYouUsingTheCommonTransitConventionCTC("No");
            WhenTheUserSelectsForWillTheTransportUseTheGoodsVehicleMovementServiceGVMS("No");
        }
    }
}