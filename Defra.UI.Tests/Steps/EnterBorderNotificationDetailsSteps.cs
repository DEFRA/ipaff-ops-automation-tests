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

        [When("the user selects {string} as the Notification type")]
        public void WhenTheUserSelectsNotificationType(string notificationType)
        {
            enterBorderNotificationDetailsPage.SelectNotificationType(notificationType);
            _scenarioContext["NotificationTypeBN"] = notificationType;
        }

        [When("the user selects {string} as the Notification basis")]
        public void WhenTheUserSelectsNotificationBasis(string notificationBasis)
        {
            enterBorderNotificationDetailsPage.SelectNotificationBasis(notificationBasis);
            _scenarioContext["NotificationBasisBN"] = notificationBasis;
        }

        [When("the user selects {string} as the Product category")]
        public void WhenTheUserSelectsProductCategory(string productCategory)
        {
            enterBorderNotificationDetailsPage.SelectProductCategory(productCategory);
            _scenarioContext["ProductCategoryBN"] = productCategory;
        }

        [When("the user enters {string} as the Product name")]
        public void WhenTheUserEntersProductName(string productName)
        {
            enterBorderNotificationDetailsPage.EnterProductName(productName);
            _scenarioContext["ProductNameBN"] = productName;
        }

        [When("the user enters {string} as the Brand name")]
        public void WhenTheUserEntersAsTheBrandName(string brandName)
        {
            enterBorderNotificationDetailsPage.EnterBrandName(brandName);
            _scenarioContext["BrandNameBN"] = brandName;
        }

        [When("the user enters {string} in the Other labelling field")]
        public void WhenTheUserEntersOtherLabelling(string otherLabelling)
        {
            enterBorderNotificationDetailsPage.EnterOtherLabelling(otherLabelling);
            _scenarioContext["OtherLabellingBN"] = otherLabelling;
        }

        [When(@"the user enters {string} in the Other information field")]
        public void WhenTheUserEntersOtherInformation(string otherInformation)
        {
            enterBorderNotificationDetailsPage.EnterOtherInformation(otherInformation);
            _scenarioContext["OtherInformationBN"] = otherInformation;
        }

        [When("the user selects {string} under the Durability date radio options")]
        public void WhenTheUserSelectsDurabilityDateOption(string durabilityOption)
        {
            var durabilityDate = enterBorderNotificationDetailsPage.SelectDurabilityDateOption(durabilityOption);
            var durabilityDateOption = durabilityOption + " - " + durabilityDate;
            _scenarioContext["DurabilityDateBN"] = durabilityDateOption;
        }

        [When("the user selects {string} as Risk decision")]
        public void WhenTheUserSelectsRiskDecision(string riskDecision)
        {
            enterBorderNotificationDetailsPage.SelectRiskDecision(riskDecision);
            _scenarioContext["RiskDecisionBN"] = riskDecision;
        }

        [When("the user selects {string} as Impact on")]
        public void WhenTheUserSelectsImpactOn(string impactOn)
        {
            enterBorderNotificationDetailsPage.SelectImpactOn(impactOn);
            _scenarioContext["ImpactOnBN"] = impactOn;
        }

        [When("the user selects {string} as Hazard category")]
        public void WhenTheUserSelectsHazardCategory(string hazardCategory)
        {
            enterBorderNotificationDetailsPage.SelectHazardCategory(hazardCategory);
            _scenarioContext["HazardCategoryBN"] = hazardCategory;
        }

        [When("the user selects {string} as Measure taken")]
        public void WhenTheUserSelectsMeasureTaken(string measureTaken)
        {
            enterBorderNotificationDetailsPage.SelectMeasureTaken(measureTaken);
            _scenarioContext["MeasureTakenBN"] = measureTaken;
        }
    }
}