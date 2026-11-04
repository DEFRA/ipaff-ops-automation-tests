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
            Console.WriteLine($"[POSTCODE LOOKUP] Entering postcode '{postcode}' (attempt 1/{maxAttempts})");
            idmWhatsThePostcodeForYourBusinessPage.EnterPostcodeAndContinue(postcode);

            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                WaitForLookupResult(lookupWaitSeconds);

                if (idmSelectYourAddressPage.IsDisplayed())
                {
                    Console.WriteLine($"[POSTCODE LOOKUP] ✓ Address selection page displayed for '{postcode}' (attempt {attempt}) - no recovery needed");
                    return;
                }

                if (idmWhatIsYourBusinessAddressPage.IsDisplayed())
                {
                    LogRecovery("MANUAL ADDRESS FALLBACK", $"Lookup for '{postcode}' returned no address list; the manual business address form is displayed (attempt {attempt})");
                    return;
                }

                if (idmWhatsThePostcodeForYourBusinessPage.IsDisplayed() && !idmWhatsThePostcodeForYourBusinessPage.IsErrorSummaryDisplayed())
                {
                    LogRecovery("LOOKUP PENDING", $"Lookup for '{postcode}' still pending after {lookupWaitSeconds} seconds (attempt {attempt}); waiting without resubmitting");
                    continue;
                }

                if (attempt == maxAttempts)
                    break;

                if (!idmWhatsThePostcodeForYourBusinessPage.IsDisplayed())
                {
                    LogRecovery("UNEXPECTED PAGE", $"Unexpected page '{SafeCurrentUrl()}' after lookup for '{postcode}' (attempt {attempt}); reloading the postcode page");
                    if (!idmWhatsThePostcodeForYourBusinessPage.ReloadPostcodePage(postcodePageUrl))
                    {
                        LogRecovery("RELOAD FAILED", $"Could not reload the postcode page '{postcodePageUrl}' (attempt {attempt}); waiting for the next lookup result");
                        continue;
                    }
                }
                else
                {
                    LogRecovery("POSTCODE REJECTED", $"Error summary displayed for '{postcode}' (attempt {attempt})");
                }

                var previousPostcode = postcode;
                postcode = await Utils.GenerateRandomUKPostcodeAsync();
                _scenarioContext.Set(postcode, "RegistrationPostcode");
                LogRecovery("NEW POSTCODE RETRY", $"Retrying lookup with '{postcode}' after '{previousPostcode}' (attempt {attempt + 1}/{maxAttempts})");
                idmWhatsThePostcodeForYourBusinessPage.EnterPostcodeAndContinue(postcode);
            }

            Console.WriteLine($"[POSTCODE LOOKUP] ✗ Lookup failed after {maxAttempts} attempts. Last postcode: '{postcode}'");
            Assert.Fail($"Neither address selection nor manual address entry was displayed after {maxAttempts} lookup attempts. Last postcode: {postcode}");
        }

        private static void LogRecovery(string recoveryType, string message)
        {
            var line = $"[POSTCODE LOOKUP] [RECOVERY: {recoveryType}] {message}";
            Console.WriteLine(line);
            TestContext.Progress.WriteLine(line);
        }

        private string SafeCurrentUrl()
        {
            try
            {
                return idmWhatsThePostcodeForYourBusinessPage.CurrentUrl;
            }
            catch (WebDriverException)
            {
                return "<unavailable>";
            }
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
