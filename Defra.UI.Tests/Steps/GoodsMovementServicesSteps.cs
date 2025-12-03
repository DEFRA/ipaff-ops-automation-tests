using Reqnroll.BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class GoodsMovementServicesSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IGoodsMovementServicesPage? goodsMovementServicesPage => _objectContainer.IsRegistered<IGoodsMovementServicesPage>() ? _objectContainer.Resolve<IGoodsMovementServicesPage>() : null;


        public GoodsMovementServicesSteps(IObjectContainer container)
        {
            _objectContainer = container;
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
    }
}