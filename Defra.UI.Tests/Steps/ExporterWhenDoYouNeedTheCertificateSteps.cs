using Defra.UI.Tests.Pages.Interfaces;
using DocumentFormat.OpenXml.Bibliography;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ExporterWhenDoYouNeedTheCertificateSteps
    {
        private readonly IObjectContainer _objectContainer;

        private IExporterWhenDoYouNeedTheCertificatePage? exporterWhenDoYouNeedTheCertificatePage =>
            _objectContainer.IsRegistered<IExporterWhenDoYouNeedTheCertificatePage>()
                ? _objectContainer.Resolve<IExporterWhenDoYouNeedTheCertificatePage>()
                : null;

        public ExporterWhenDoYouNeedTheCertificateSteps(IObjectContainer container)
        {
            _objectContainer = container;
        }

        [Then("the When do you need the certificate? page is displayed")]
        public void ThenTheWhenDoYouNeedTheCertificatePageIsDisplayed()
        {
            Assert.True(exporterWhenDoYouNeedTheCertificatePage?.IsPageLoaded(), "When do you need the certificate? page is not displayed");
        }

        [When("the user enters a valid date and time at least 24 hours in the future and clicks the Continue button")]
        public void WhenTheUserEntersAValidDateAndTimeAtLeast24HoursInTheFutureAndClicksTheContinueButton()
        {
            exporterWhenDoYouNeedTheCertificatePage?.EnterNeededDateAndTime(
                DateTime.Today.AddDays(3),
                "11:00",
                "am");

            exporterWhenDoYouNeedTheCertificatePage?.ClickContinueButton();
        }

        [When("the user enters a valid date for when the certificate is needed at least {int} days in the future")]
        public void WhenTheUserEntersAValidDateForWhenTheCertificateIsNeededAtLeastDaysInTheFuture(int days)
        {
            exporterWhenDoYouNeedTheCertificatePage?.EnterNeededDate(DateTime.Today.AddDays(days));
        }

        [When("the user clicks the Continue button on the When do you need the certificate? page")]
        public void WhenTheUserClicksTheContinueButtonOnTheWhenDoYouNeedTheCertificatePage()
        {
            exporterWhenDoYouNeedTheCertificatePage?.ClickContinueButton();
        }
    }
}