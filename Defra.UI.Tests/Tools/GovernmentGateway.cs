using Defra.UI.Tests.Contracts;
using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.HelperMethods;
using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Tools
{
    public sealed class GovernmentGateway
    {
        private static GovernmentGateway _instance;
        private static readonly object _lock = new object();

        private LoginDetails _cachedValue;
        private bool _isMethodCalled = false;

        private ScenarioContext? _scenarioContext;
        private IObjectContainer _objectContainer;
        private ISignInPage? signin => _objectContainer.IsRegistered<ISignInPage>() ? _objectContainer.Resolve<ISignInPage>() : null;
        private IEmailSignUpPage? emailSignUpPage => _objectContainer.IsRegistered<IEmailSignUpPage>() ? _objectContainer.Resolve<IEmailSignUpPage>() : null;
        private IGovernmentGatewayTypePage? governmentGatewayTypePage => _objectContainer.IsRegistered<IGovernmentGatewayTypePage>() ? _objectContainer.Resolve<IGovernmentGatewayTypePage>() : null;
        private IHomePage? homePage => _objectContainer.IsRegistered<IHomePage>() ? _objectContainer.Resolve<IHomePage>() : null;
        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private IUrlBuilder? urlBuilder => _objectContainer.IsRegistered<IUrlBuilder>() ? _objectContainer.Resolve<IUrlBuilder>() : null;
        private IFetchCodeFromEmail? fetchCodeFromEmail => _objectContainer.IsRegistered<IFetchCodeFromEmail>() ? _objectContainer.Resolve<IFetchCodeFromEmail>() : null;
        private IUserObject? userObject => _objectContainer.IsRegistered<IUserObject>() ? _objectContainer.Resolve<IUserObject>() : null;

        private GovernmentGateway(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
            _scenarioContext = objectContainer.Resolve<ScenarioContext>();
        }

        public static void Initialize(IObjectContainer objectContainer)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new GovernmentGateway(objectContainer);
                    }
                }
            }
        }

        public static GovernmentGateway Instance
        {
            get
            {
                if (_instance == null)
                    throw new InvalidOperationException("GovernmentGateway not initialized.");

                return _instance;
            }
        }

        public LoginDetails GetUserDetails()
        {
            if (!_isMethodCalled)
            {
                lock (_lock)
                {
                    if (!_isMethodCalled)
                    {
                        _cachedValue = GenerateID();
                        _isMethodCalled = true;
                    }
                }
            }
            return _cachedValue;
        }

        public LoginDetails GetUserDetailsFromFile(string role = "")
        {
            if (_cachedValue == null)
            {
                var user = userObject?.GetUser("IPAFF", role);

                _cachedValue = new LoginDetails
                {
                    GovernmentGatewayID = user?.UserName,
                    Secret = user?.Credential,
                };
            }

            return _cachedValue;
        }

        private LoginDetails GenerateID()
        {
            var url = urlBuilder?.Default().BuildApp();
            _driver?.Navigate().GoToUrl(url);

            governmentGatewayTypePage?.SelectLoginType("Sign in with Government Gateway");
            governmentGatewayTypePage?.ClickContinueButton();

            signin?.ClickCreateSignInDetailsLink();

            var emailText = $"ipaffautomation{DateTime.Now.ToString("yyyyMMddHHmmss")}";
            var emailAddress = $"{emailText}@{fetchCodeFromEmail?.DomainName}";
            var secret = "G0vernmen+";

            emailSignUpPage?.EnterEmailAddress(emailAddress);
            Thread.Sleep(3000);
            emailSignUpPage?.ClickContinueButton();
            
            var code = Task.Run(async () => await fetchCodeFromEmail?.GetCodeFromEmail(emailText)).Result;

            emailSignUpPage?.EnterConfirmationCode(code);
            emailSignUpPage?.ClickContinueButton();
            _driver.Wait(5);
            emailSignUpPage?.ClickContinueButton();

            emailSignUpPage?.EnterFullName("Pets Automation");
            emailSignUpPage?.ClickContinueButton();

            emailSignUpPage?.EnterThePassword(secret);
            emailSignUpPage?.ClickContinueButton();
         
            var ggid = emailSignUpPage?.GetGGID();
            Assert.IsNotEmpty(ggid);

            emailSignUpPage?.ClickContinueButton();

            _driver.Wait(2);
            emailSignUpPage?.ClickContinueButton();
            _driver.Wait(2);
            emailSignUpPage?.ClickContinueButton();
            _driver.Wait(2);
            emailSignUpPage?.ClickContinueButton();

            emailSignUpPage?.SelectIndividualUser();
            emailSignUpPage?.ClickContinueButton();

            emailSignUpPage?.EnterFirstAndLastName("Pets", "Automation");
            emailSignUpPage?.ClickContinueButton();

            emailSignUpPage?.EnterTelephoneNumber("07539928765");
            emailSignUpPage?.ClickContinueButton();

            emailSignUpPage?.EnterPostCode("OX1 1AF");
            emailSignUpPage?.ClickContinueButton();

            emailSignUpPage?.SelectAddress();
            emailSignUpPage?.ClickContinueButton();

            emailSignUpPage?.EnterMemorableWordAndHint("OpsPetsTesting", "OpsPetsTesting");
            emailSignUpPage?.ClickContinueButton();
            _driver.Wait(2);
            emailSignUpPage?.ClickContinueButton();

            _driver.Wait(10);
            Assert.True(homePage?.IsPageLoaded(), "Apply for a pet travel document not loaded");
            Assert.True(signin?.IsSignedOut(), "Not able to sign out");

            return new LoginDetails
            {
                EmailText = emailText,
                EmailAddress = emailAddress,
                ConfirmationCode = code,
                GovernmentGatewayID = ggid,
                Secret = secret
            };
        }
    }
}
