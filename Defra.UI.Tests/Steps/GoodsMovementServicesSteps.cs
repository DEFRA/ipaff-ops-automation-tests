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
            // Remove "Transport After BCP" keys since GVMS page means no airplane transport
            // These keys would have been set in a previous amendment and are no longer valid
            RemoveTransportAfterBCPKeys();

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

        /// <summary>
        /// Removes Transport After BCP context keys when GVMS page is displayed
        /// (indicating non-airplane transport mode)
        /// </summary>
        private void RemoveTransportAfterBCPKeys()
        {
            var keysToRemove = new[]
            {
                "MeansOfTransportAfterBCP",
                "TransportIdentificationAfterBCP",
                "TransportDocumentReferenceAfterBCP",
                "DepartureDateFromBCP",
                "DepartureTimeFromBCP"
            };

            foreach (var key in keysToRemove)
            {
                if (_scenarioContext.ContainsKey(key))
                {
                    _scenarioContext.Remove(key);
                }
            }
        }
    }
}