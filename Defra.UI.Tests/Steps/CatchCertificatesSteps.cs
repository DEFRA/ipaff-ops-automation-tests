using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;


namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class CatchCertificatesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ICatchCertificatesPage? catchCertificatesPage => _objectContainer.IsRegistered<ICatchCertificatesPage>() ? _objectContainer.Resolve<ICatchCertificatesPage>() : null;

        public CatchCertificatesSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }


        [Then("the Catch cerificates page should be displayed")]
        public void ThenTheCatchCerificatesPageShouldBeDisplayed()
        {
            Assert.True(catchCertificatesPage?.IsPageLoaded(), "Documents Catch certificates page not loaded");
        }


        [Then("the user selects {string} option for add catch certificate")]
        public void WhenTheUserSelectsOption(string option)
        {
            catchCertificatesPage?.SelectAddCatchCertificate(option);
        }
    }
}