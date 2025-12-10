using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;

using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class NominatedContactsSteps
    {
        private readonly IObjectContainer _objectContainer;

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