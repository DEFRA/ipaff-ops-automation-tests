using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class TransportContactsSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private ITransportContactsPage? transportContactsPage => _objectContainer.IsRegistered<ITransportContactsPage>() ? _objectContainer.Resolve<ITransportContactsPage>() : null;


        public TransportContactsSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("the Should we notify any transport contacts about inspections? page should be displayed")]
        [Then("the Transport Contacts page should be displayed")]
        public void ThenTheTransportContactsPageShouldBeDisplayed()
        {
            Assert.True(transportContactsPage?.IsPageLoaded(), "Transport Contacts page not loaded");
        }

        [When("the user selects {string} for Should we notify any transport contacts about inspections?")]
        public void WhenTheUserSelectsForShouldWeNotifyAnyTransportContactsAboutInspections(string option)
        {
            transportContactsPage?.SelectTransportContactNotification(option);
            _scenarioContext["ShouldNotifyTransportContacts"] = option;
        }
    }
}