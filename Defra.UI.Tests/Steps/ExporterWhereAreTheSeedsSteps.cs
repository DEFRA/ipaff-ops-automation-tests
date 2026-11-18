using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterWhereAreTheSeedsSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IExporterWhereAreTheSeedsPage? exporterWhereAreTheSeedsPage =>
            _objectContainer.IsRegistered<IExporterWhereAreTheSeedsPage>()
                ? _objectContainer.Resolve<IExporterWhereAreTheSeedsPage>()
                : null;

        public ExporterWhereAreTheSeedsSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the Where are the seeds? page is displayed")]
        public void ThenTheWhereAreTheSeedsPageIsDisplayed()
        {
            Assert.True(exporterWhereAreTheSeedsPage?.IsPageLoaded(), "Where are the seeds? page is not displayed");
        }
    }
}
