using Reqnroll.BoDi;
using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class GoodsMovementServicesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
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
        }

        [When("the user selects {string} for Will the transport use the Goods Vehicle Movement Service \\(GVMS)?")]
        public void WhenTheUserSelectsForWillTheTransportUseTheGoodsVehicleMovementServiceGVMS(string option)
        {
            goodsMovementServicesPage?.GVMSToMoveGoods(option);
            _scenarioContext.Add("IsGVMS", option);
        }
    }
}