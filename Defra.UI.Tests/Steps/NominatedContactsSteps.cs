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
    public class NominatedContactsSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private INominatedContactsPage? nominatedContactsPage => _objectContainer.IsRegistered<INominatedContactsPage>() ? _objectContainer.Resolve<INominatedContactsPage>() : null;


        public NominatedContactsSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }


        [Then("the Nominated contacts page should be displayed")]
        public void ThenTheNominatedContactsPageShouldBeDisplayed()
        {
            Assert.True(nominatedContactsPage?.IsPageLoaded(), "Contacts Nominated contacts (optional) page not loaded");
        }
    }
}