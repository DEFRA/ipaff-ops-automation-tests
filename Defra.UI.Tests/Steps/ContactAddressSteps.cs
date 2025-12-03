using Reqnroll.BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class ContactAddressSteps
    {
        private readonly IObjectContainer _objectContainer;

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
    }
}