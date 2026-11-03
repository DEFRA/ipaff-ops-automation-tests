using Defra.UI.Tests.Tools;
using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmWhatsThePostcodeForYourBusinessSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private IIdmWhatsThePostcodeForYourBusinessPage idmWhatsThePostcodeForYourBusinessPage => _objectContainer.Resolve<IIdmWhatsThePostcodeForYourBusinessPage>();
        private IIdmSelectYourAddressPage idmSelectYourAddressPage => _objectContainer.Resolve<IIdmSelectYourAddressPage>();
        private IIdmWhatIsYourBusinessAddressPage idmWhatIsYourBusinessAddressPage => _objectContainer.Resolve<IIdmWhatIsYourBusinessAddressPage>();
        public IdmWhatsThePostcodeForYourBusinessSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the What’s the postcode for your business? page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(idmWhatsThePostcodeForYourBusinessPage.IsPageLoaded(), Is.True);

        [When("the user enters the generated postcode for the business and continues")]
        public async Task WhenPostcodeIsEntered()
        {
            const int maxAttempts = 5;
            const int lookupWaitSeconds = 45;
            var postcodePageUrl = idmWhatsThePostcodeForYourBusinessPage.CurrentUrl;
            var postcode = await Utils.GenerateRandomUKPostcodeAsync();
            _scenarioContext.Set(postcode, "RegistrationPostcode");
            idmWhatsThePostcodeForYourBusinessPage.EnterPostcodeAndContinue(postcode);

            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                WaitForLookupResult(lookupWaitSeconds);

                if (idmSelectYourAddressPage.IsDisplayed())
                    return;

                if (idmWhatIsYourBusinessAddressPage.IsDisplayed())
                {
                    TestContext.Progress.WriteLine($"[RECOVERY] Manual address entry displayed for {postcode} (attempt {attempt}).");
                    return;
                }

                if (idmWhatsThePostcodeForYourBusinessPage.IsDisplayed() && !idmWhatsThePostcodeForYourBusinessPage.IsErrorSummaryDisplayed())
                {
                    TestContext.Progress.WriteLine($"[RECOVERY] Postcode lookup still pending after {lookupWaitSeconds} seconds (attempt {attempt}); waiting without resubmitting.");
                    continue;
                }

                if (attempt == maxAttempts)
                    break;

                if (!idmWhatsThePostcodeForYourBusinessPage.IsDisplayed())
                {
                    TestContext.Progress.WriteLine($"[RECOVERY] Unexpected page (attempt {attempt}); reloading the postcode page.");
                    if (!idmWhatsThePostcodeForYourBusinessPage.ReloadPostcodePage(postcodePageUrl))
                        continue;
                }

                var previousPostcode = postcode;
                postcode = await Utils.GenerateRandomUKPostcodeAsync();
                _scenarioContext.Set(postcode, "RegistrationPostcode");
                TestContext.Progress.WriteLine($"[RECOVERY] Retrying postcode lookup with {postcode} after {previousPostcode} (attempt {attempt + 1}).");
                idmWhatsThePostcodeForYourBusinessPage.EnterPostcodeAndContinue(postcode);
            }

            Assert.Fail($"Neither address selection nor manual address entry was displayed after {maxAttempts} lookup attempts. Last postcode: {postcode}");
        }

        private void WaitForLookupResult(int timeoutSeconds) =>
            SpinWait.SpinUntil(() =>
            {
                try
                {
                    return idmSelectYourAddressPage.IsDisplayed()
                        || idmWhatIsYourBusinessAddressPage.IsDisplayed()
                        || idmWhatsThePostcodeForYourBusinessPage.IsErrorSummaryDisplayed();
                }
                catch (WebDriverException)
                {
                    return false;
                }
            }, TimeSpan.FromSeconds(timeoutSeconds));
    }
}
