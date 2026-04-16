using Reqnroll.BoDi;
using NUnit.Framework;
using Reqnroll;
using Defra.UI.Tests.Pages.Interfaces;
using OpenQA.Selenium;
using System.Reflection;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class SignOutSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private ISignOutPage? signOutPage => _objectContainer.IsRegistered<ISignOutPage>() ? _objectContainer.Resolve<ISignOutPage>() : null;

        public SignOutSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [When("the user logs out of IPAFFS Part {int}")]
        [When("the user logs out of Border notifications in IPAFFS Part {int}")]
        public void WhenTheUserLogsOutOfIPAFFSPart(int partNumber)
        {
            signOutPage?.SignedOut();
        }

        [Then("the user should be logged out successfully")]
        public void ThenTheUserShouldBeLoggedOutSuccessfully()
        {
            Assert.True(signOutPage?.VerifySignedOutPage(), "Signed out page not loaded");
        }

        [Then("I close the browser")]
        public void ThenICloseTheBrowser()
        {
            signOutPage?.CloseBrowser();
        }

        /// <summary>
        /// Re-registers the Dynamics browser driver (focused on the IPAFFS tab) into
        /// Assembly 1's BoDi container, replacing the original Browser 1 IWebDriver.
        /// All subsequent IPAFFS page step definitions will automatically use this driver.
        ///
        /// BoDi locks registrations once resolved, so this method bypasses the public API
        /// by writing directly into BoDi's internal <c>_objectPool</c> and <c>_registrations</c>
        /// dictionaries via reflection, and clears <c>_resolvedKeys</c> to allow future resolves.
        ///
        /// Must be called after "When I click IPAFFS from the header ribbon" and before any
        /// IPAFFS page interaction steps.
        /// </summary>
        [When("I switch to the IPAFFS tab")]
        public void WhenISwitchToTheIpaffsTab()
        {
            if (!_scenarioContext.TryGetValue("DynamicsIpaffsDriver", out IWebDriver dynamicsDriver)
                || dynamicsDriver == null)
            {
                throw new InvalidOperationException(
                    "No Dynamics IPAFFS driver found in ScenarioContext. " +
                    "Ensure 'When I click IPAFFS from the header ribbon' ran before this step.");
            }

            SwapDriver(_objectContainer, dynamicsDriver);
        }

        private static void SwapDriver(IObjectContainer container, IWebDriver newDriver)
        {
            var containerType = container.GetType();
            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;

            var objectPoolField = containerType.GetField("_objectPool", flags)
                ?? throw new InvalidOperationException("BoDi field '_objectPool' not found.");

            var registrationsField = containerType.GetField("_registrations", flags)
                ?? throw new InvalidOperationException("BoDi field '_registrations' not found.");

            var resolvedKeysField = containerType.GetField("_resolvedKeys", flags)
                ?? throw new InvalidOperationException("BoDi field '_resolvedKeys' not found.");

            var objectPool = (System.Collections.IDictionary)objectPoolField.GetValue(container)!;
            var registrations = (System.Collections.IDictionary)registrationsField.GetValue(container)!;
            var resolvedKeys = resolvedKeysField.GetValue(container) as System.Collections.IList;

            // BoDi's RegistrationKey is a struct with a public readonly 'Type' field
            static Type? GetKeyType(object key)
            {
                var fi = key.GetType().GetField("Type",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                return fi?.GetValue(key) as Type;
            }

            // _objectPool values are wrapped in NonDisposableWrapper — match the same type
            // for the new driver so BoDi's disposal logic handles it consistently
            var nonDisposableWrapperType = objectPool.Values
                .Cast<object>()
                .FirstOrDefault(v => v?.GetType().Name == "NonDisposableWrapper")
                ?.GetType();

            object newValue = newDriver;
            if (nonDisposableWrapperType != null)
            {
                var wrapperCtor =
                    nonDisposableWrapperType.GetConstructor(flags | BindingFlags.Public, null, [typeof(object)], null)
                    ?? nonDisposableWrapperType.GetConstructors(flags | BindingFlags.Public).FirstOrDefault();

                if (wrapperCtor != null)
                    newValue = wrapperCtor.Invoke([newDriver]);
            }

            // Replace resolved instances (covers both IWebDriver and ChromeDriver keys)
            foreach (var key in objectPool.Keys.Cast<object>()
                         .Where(k => GetKeyType(k) is Type t && typeof(IWebDriver).IsAssignableFrom(t))
                         .ToList())
            {
                objectPool[key] = newValue;
            }

            // Update the InstanceRegistration so that any new Resolve call also returns the new driver
            foreach (var key in registrations.Keys.Cast<object>()
                         .Where(k => GetKeyType(k) is Type t && typeof(IWebDriver).IsAssignableFrom(t))
                         .ToList())
            {
                var reg = registrations[key];
                if (reg == null) continue;

                var instanceField = reg.GetType()
                    .GetFields(flags | BindingFlags.Public)
                    .FirstOrDefault(f => typeof(object).IsAssignableFrom(f.FieldType)
                                      && f.Name.Contains("instance", StringComparison.OrdinalIgnoreCase));

                instanceField?.SetValue(reg, newDriver);
            }

            // Clear resolved keys so BoDi does not throw AssertNotResolved on future Resolve calls
            if (resolvedKeys != null)
            {
                foreach (var key in resolvedKeys.Cast<object>()
                             .Where(k => GetKeyType(k) is Type t && typeof(IWebDriver).IsAssignableFrom(t))
                             .ToList())
                {
                    resolvedKeys.Remove(key);
                }
            }
        }

        [Then("the Inspector is logged out of IPAFFS successfully")]
        public void ThenTheInspectorIsLoggedOutOfIPAFFSSuccessfully()
        {
            Assert.True(
                signOutPage?.VerifyInspectorSignedOutPage(),
                "Inspector sign-out page was not displayed — expected 'You signed out of your account'.");
        }

        [When("the user closes the IPAFFS tab")]
        public void WhenTheUserClosesTheIpaffsTab()
        {
            signOutPage?.CloseCurrentTab();
        }
    }
}