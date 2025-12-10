using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;

namespace Defra.UI.Tests.Steps.CP
{
    [Binding]
    public class IUUSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IIUUPage? iuuPage => _objectContainer.IsRegistered<IIUUPage>() ? _objectContainer.Resolve<IIUUPage>() : null;


        public IUUSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }


        [Then("the IUU page should be displayed")]
        public void ThenTheIUUPageShouldBeDisplayed()
        {
            Assert.True(iuuPage?.IsPageLoaded(), "IUU Illegal, unreported and unregulated fishing page not loaded");
        }

        [When("the user selects {string} and sub-option as {string} for the IUU check")]
        public void WhenTheUserSelectsAndSub_OptionAsForTheIUUCheck(string option, string subOption)
        {
            iuuPage?.SelectRecordIUUCheckOption(option, subOption);
        }
    }
}