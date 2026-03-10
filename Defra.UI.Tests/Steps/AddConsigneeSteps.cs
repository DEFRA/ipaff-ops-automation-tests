using Defra.UI.Tests.Pages.Interfaces;
using Reqnroll;
using Reqnroll.BoDi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class AddConsigneeSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IAddConsigneePage? addConsigneePage => _objectContainer.IsRegistered<IAddConsigneePage>() ? _objectContainer.Resolve<IAddConsigneePage>() : null;
        private ITheConsigneeHasBeenCreatedPage? theConsigneeHasBeenCreatePage => _objectContainer.IsRegistered<ITheConsigneeHasBeenCreatedPage>() ? _objectContainer.Resolve<ITheConsigneeHasBeenCreatedPage>() : null;

        public AddConsigneeSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("Add consignee page should be displayed")]
        public void TheAddConsigneePageShouldBeDisplayed()
        {
            addConsigneePage?.IsPageLoaded();
        }

        [When("the user enters Consignee name as {string}")]
        public void WhenTheUserEntersConsigneeNameAs(string name)
        {
            addConsigneePage?.EnterConsigneeName(name);
        }

        [When("the user enters Consignee address as {string}")]
        public void WhenTheUserEntersConsigneeAddressAs(string address)
        {
            addConsigneePage?.EnterConsigneeAddress(address);
        }

        [When("the user enters Consignee City as {string}")]
        public void WhenTheUserEntersConsigneeCityAs(string city)
        {
            addConsigneePage?.EnterConsigneeCityOrTown(city);
        }

        [When("the user enters the Consignee Postcode as {string}")]
        public void WhenTheUserEntersConsigneePostcodeAs(string postcode)
        {
            addConsigneePage?.EnterConsigneePostCode(postcode);
        }

        [When("the user enters the Consignee Telephone number as {string}")]
        public void WhenTheUserEntersConsigneeTelephoneAs(string telephoneNumber)
        {
            addConsigneePage?.EnterConsigneeTelephone(telephoneNumber);
        }

        [When("the user enters the Consignee Country as {string}")]
        public void WhenTheUserEntersConsigneeCountryAs(string country)
        {
            addConsigneePage?.EnterConsigneeCountry(country);
        }

        [When("the user enters the Consignee Email as {string}")]
        public void WhenTheUserEntersConsigneeEmailAs(string email)
        {
            addConsigneePage?.EnterConsigneeEmail(email);
        }

        [Then("The consignee has been created page is displayed")]
        public void ThenTheConsigneeHasBeenCreatedPageIsDisplayed()
        {
            theConsigneeHasBeenCreatePage?.IsPageLoaded();
        }
        
        [When("the user clicks Add to notification button")]
        public void WhenTheUserClicksAddToNotificationButton()
        {
            theConsigneeHasBeenCreatePage?.clickAddToNotificationButton();
        }
    }
}
