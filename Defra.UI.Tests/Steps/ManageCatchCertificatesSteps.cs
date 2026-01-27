using Defra.UI.Tests.Pages.Classes;
using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps
{
    [Binding]
    public class ManageCatchCertificatesSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IManageCatchCertificatesPage? manageCatchCertificates => _objectContainer.IsRegistered<IManageCatchCertificatesPage>() ? _objectContainer.Resolve<IManageCatchCertificatesPage>() : null;

        public ManageCatchCertificatesSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [Then("Manage catch certificates page is displayed")]
        public void ThenTheDashboardShouldBeDisplayed()
        {
            Assert.True(manageCatchCertificates?.IsPageLoaded("Manage catch certificates"), "Manage catch certificates page is not displayed");
        }
        
        [Then("the user verifies there are {string} certificates attached")]
        public void ThenTheUserVerifiesThereAreCertificatesAttached(string numberOfCertificates)
        {
            Assert.True(manageCatchCertificates?.VerifyNumberOfCertificates(numberOfCertificates), "The number of certificates are not equal to "+ numberOfCertificates);
        }

        [When("the user selects the {string} option for Do you need to upload more catch certificates?")]
        public void WhenTheUserSelectsOption(string option)
        {
            manageCatchCertificates?.SelectOption(option.ToLower());
        }
    }
}
