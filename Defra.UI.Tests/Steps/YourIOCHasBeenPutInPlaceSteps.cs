using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class YourIOCHasBeenPutInPlaceSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IYourIOCHasBeenPutInPlacePage? yourIOCHasBeenPutInPlacePage =>
            _objectContainer.IsRegistered<IYourIOCHasBeenPutInPlacePage>()
                ? _objectContainer.Resolve<IYourIOCHasBeenPutInPlacePage>()
                : null;

        public YourIOCHasBeenPutInPlaceSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Your intensified official control has been put in place page should be displayed")]
        public void ThenTheYourIntensifiedOfficialControlHasBeenPutInPlacePageShouldBeDisplayed()
        {
            Assert.True(
                yourIOCHasBeenPutInPlacePage?.IsPageLoaded(),
                "Your intensified official control has been put in place page is not loaded");
        }

        [Then("the Intensified official control number should be displayed in the correct format")]
        public void ThenTheIntensifiedOfficialControlNumberShouldBeDisplayedInTheCorrectFormat()
        {
            var iocNumber = yourIOCHasBeenPutInPlacePage?.GetIntensifiedOfficialControlNumber();

            _scenarioContext["IntensifiedOfficialControlNumber"] = iocNumber;

            Assert.True(
                yourIOCHasBeenPutInPlacePage?.IsIntensifiedOfficialControlNumberInCorrectFormat(),
                $"Intensified official control number '{iocNumber}' is not in the expected format 'IOC.202x.xxxxxxx'");
        }
    }
}