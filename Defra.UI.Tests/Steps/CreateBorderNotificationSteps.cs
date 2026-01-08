using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CreateBorderNotificationSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ICreateBorderNotificationPage? createBorderNotificationPage => _objectContainer.IsRegistered<ICreateBorderNotificationPage>() ? _objectContainer.Resolve<ICreateBorderNotificationPage>() : null;

        public CreateBorderNotificationSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then("Enter the details of the border notification page should be displayed")]
        public void ThenEnterTheDetailsOfTheBorderNotificationPageShouldBeDisplayed()
        {
            Assert.True(createBorderNotificationPage?.IsPageLoaded(), "Enter the details of the border notification page is not loaded");
        }
    }
}
