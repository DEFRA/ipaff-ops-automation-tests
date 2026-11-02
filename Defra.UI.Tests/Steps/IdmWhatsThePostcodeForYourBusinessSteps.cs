using Defra.UI.Tests.Tools;
using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmWhatsThePostcodeForYourBusinessSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private IIdmWhatsThePostcodeForYourBusinessPage page => _objectContainer.Resolve<IIdmWhatsThePostcodeForYourBusinessPage>();
        private IIdmSelectYourAddressPage selectAddressPage => _objectContainer.Resolve<IIdmSelectYourAddressPage>();
        private IIdmWhatIsYourBusinessAddressPage manualAddressPage => _objectContainer.Resolve<IIdmWhatIsYourBusinessAddressPage>();
        public IdmWhatsThePostcodeForYourBusinessSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the What’s the postcode for your business? page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(page.IsPageLoaded(), Is.True);

        [When("the user enters the generated postcode for the business and continues")]
        public async Task WhenPostcodeIsEntered()
        {
            const int maxAttempts = 5;
            const int lookupWaitSeconds = 45;
            var business = _scenarioContext.Get<OperatorDetails>("RegistrationBusiness");
            var postcodePageUrl = page.CurrentUrl;
            business.Postcode = await Utils.GenerateRandomUKPostcodeAsync();
            page.EnterPostcodeAndContinue(business.Postcode);

            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                if (selectAddressPage.WaitForPage(lookupWaitSeconds))
                    return;

                if (manualAddressPage.IsDisplayed())
                {
                    TestContext.Progress.WriteLine($"[RECOVERY] Manual address entry displayed for {business.Postcode} (attempt {attempt}).");
                    return;
                }

                if (page.IsDisplayed() && !page.IsErrorSummaryDisplayed())
                {
                    TestContext.Progress.WriteLine($"[RECOVERY] Postcode lookup still pending after {lookupWaitSeconds} seconds (attempt {attempt}); waiting without resubmitting.");
                    continue;
                }

                if (attempt == maxAttempts)
                    break;

                if (!page.IsDisplayed())
                {
                    TestContext.Progress.WriteLine($"[RECOVERY] Unexpected page (attempt {attempt}); reloading the postcode page.");
                    if (!page.ReloadPostcodePage(postcodePageUrl))
                        continue;
                }

                var previousPostcode = business.Postcode;
                business.Postcode = await Utils.GenerateRandomUKPostcodeAsync();
                TestContext.Progress.WriteLine($"[RECOVERY] Retrying postcode lookup with {business.Postcode} after {previousPostcode} (attempt {attempt + 1}).");
                page.EnterPostcodeAndContinue(business.Postcode);
            }

            Assert.Fail($"Neither address selection nor manual address entry was displayed after {maxAttempts} lookup attempts. Last postcode: {business.Postcode}");
        }
    }
}
