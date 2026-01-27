using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
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
    public class UploadCatchCertificatesSteps
    {

        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;


        private IUploadCatchCertificatesPage? uploadCatchCertificates => _objectContainer.IsRegistered<IUploadCatchCertificatesPage>() ? _objectContainer.Resolve<IUploadCatchCertificatesPage>() : null;

        public UploadCatchCertificatesSteps(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _objectContainer = container;
            _scenarioContext = scenarioContext;
        }

        [Then("Upload catch certificates page is displayed")]
        public void ThenTheDashboardShouldBeDisplayed()
        {
            Assert.True(uploadCatchCertificates?.IsPageLoaded("Upload catch certificates"), "Upload catch certificates page is not displayed");
        }
    }
}
