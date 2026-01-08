using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class EnterBorderNotificationDetailsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IEnterBorderNotificationDetailsPage? enterBorderNotificationDetailsPage => _objectContainer.IsRegistered<IEnterBorderNotificationDetailsPage>() ? _objectContainer.Resolve<IEnterBorderNotificationDetailsPage>() : null;

        public EnterBorderNotificationDetailsSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("Enter the details of the border notification page should be displayed")]
        public void ThenEnterTheDetailsOfTheBorderNotificationPageShouldBeDisplayed()
        {
            Assert.True(enterBorderNotificationDetailsPage?.IsPageLoaded(), "Enter the details of the border notification page not loaded");
        }


        [Then("the user enters the details as {string} {string} {string} {string} {string} {string} {string} {string} {string} {string} {string} {string}")]
        public void ThenTheUserEntersTheDetailsAs(string type, string basis, string category, string product, string brand, string label, string otherInfo, string dateOption, string decision, string impact, string hazard, string measure)
        {
            enterBorderNotificationDetailsPage?.SelectNotificationDetails(type, basis);
            enterBorderNotificationDetailsPage?.SelectProductDetails(category, product, brand);
            enterBorderNotificationDetailsPage?.SelectOtherDetails(label, otherInfo, dateOption);
            enterBorderNotificationDetailsPage?.SelectRiskDetails(decision, impact, hazard, measure);
        }
    }
}