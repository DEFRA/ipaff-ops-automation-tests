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
    public class ContactAddressSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private IContactAddressPage? contactAddressPage => _objectContainer.IsRegistered<IContactAddressPage>() ? _objectContainer.Resolve<IContactAddressPage>() : null;


        public ContactAddressSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Contact address for consignment page should be displayed")]
        public void ThenTheContactAddressForConsignmentPageShouldBeDisplayed()
        {
            Assert.True(contactAddressPage?.IsPageLoaded(), "Complete notification Contact address for consignment page not loaded");
        }

        [Then("the Contacts - Contact address for consignment page should be displayed")]
        public void ThenTheContacts_ContactAddressForConsignmentPageShouldBeDisplayed()
        {
            Assert.True(contactAddressPage?.IsContactAddressForConsignmentPageLoaded(), "Complete notification Contact address for consignment page not loaded");
        }
    }
}