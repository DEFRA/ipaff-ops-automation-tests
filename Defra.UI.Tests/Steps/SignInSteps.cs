using AventStack.ExtentReports.Gherkin.Model;
using Defra.UI.Tests.Data.Users;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class SignInSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private IUrlBuilder? urlBuilder => _objectContainer.IsRegistered<IUrlBuilder>() ? _objectContainer.Resolve<IUrlBuilder>() : null;
        private ISignInPage? _signInPage => _objectContainer.IsRegistered<ISignInPage>() ? _objectContainer.Resolve<ISignInPage>() : null;
        private IUserObject? UserObject => _objectContainer.IsRegistered<IUserObject>() ? _objectContainer.Resolve<IUserObject>() : null;
        private IGovernmentGatewayTypePage? governmentGatewayTypePage => _objectContainer.IsRegistered<IGovernmentGatewayTypePage>() ? _objectContainer.Resolve<IGovernmentGatewayTypePage>() : null;

        public SignInSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }
       
        [Given(@"that I navigate to the IPAFF application")]
        [When(@"I navigate to the IPAFF application")]
        [When("the user logs back into IPAFFS application")]
        public void GivenThatINavigateToThePortCheckerApplication()
        {
            var url = urlBuilder.Default().BuildApp();
            _driver?.Navigate().GoToUrl(url);
        }

        [When(@"the user navigate to the BTMS application")]
        public void GivenTheUserINavigateToTheBTMSApplication()
        {
            var url = urlBuilder.BTMSDefault().BuildBTMSApp();
            _driver?.Navigate().GoToUrl(url);
        }

        [When(@"I navigate to the IPAFF Inspector application")]
        [Given(@"that I navigate to the IPAFF Inspector application")]
        public void GivenThatINavigateToTheIPAFFInspectorApplication()
        {
            var url = urlBuilder.InspectorDefault().BuildInspectorApp();
            _driver?.Navigate().GoToUrl(url);
        }

        [When(@"I click signin button on port checker application")]
        [Given(@"I click signin button on port checker application")]
        public void GivenIClickSigninButtonOnPortCheckerApplication()
        {
            _signInPage?.ClickSignInButton();
        }

        [Then("I should see type of Gateway login page")]
        public void ThenIShouldSeeTypeOfGatewayLoginPage()
        {
            Assert.True(governmentGatewayTypePage?.IsPageLoaded("How do you want to sign in?"), "How do you want to sign in? page not loaded");
        }

        [Then("I have selected {string} as login type")]
        public void ThenIHaveSelectedAsLoginType(string loginType)
        {
            governmentGatewayTypePage?.SelectLoginType(loginType);
        }

        [When("I click Continue button from How do you want to sign in page")]
        public void WhenIClickContinueButtonFromHowDoYouWantToSignInPage()
        {
            governmentGatewayTypePage?.ClickContinueButton();
        }

        [Then(@"I should redirected to the BTMS Sign in using Government Gateway page")]
        [Then(@"I should redirected to the IPAFF Sign in using Government Gateway page")]
        public void ThenIShouldRedirectedToTheIPAFFSignInUsingGovernmentGatewayPage()
        {
            Assert.True(_signInPage?.IsPageLoaded(), "Application page not loaded");
        }

        [When(@"I have provided the IPAFF credentials and signin")]
        public void WhenIHaveProvidedTheIPAFFCredentialsAndSignin()
        {
            var jsonData = UserObject?.GetUser("IPAFF", "User");
            var userObject = new User
            {
                UserName = jsonData.UserName,
                Credential = jsonData.Credential
            };

            _signInPage?.SignIn(userObject.UserName, userObject.Credential);
        }

        [When(@"I have provided the BTMS credentials and signin")]
        public void WhenIHaveProvidedTheBTMSCredentialsAndSignin()
        {
            var jsonData = UserObject?.GetUser("IPAFF", "BTMS");
            var userObject = new User
            {
                UserName = jsonData.UserName,
                Credential = jsonData.Credential
            };

            _signInPage?.SignIn(userObject.UserName, userObject.Credential);
        }

        [When(@"I have provided the IPAFF Inspector credentials and signin")]
        public void WhenIHaveProvidedTheIPAFFInspectorCredentialsAndSignin()
        {
            var jsonData = UserObject?.GetUser("IPAFF", "Inspector");
            var userObject = new User
            {
                UserName = jsonData.UserName,
                Credential = jsonData.Credential
            };

            _signInPage?.SignIn(userObject.UserName, userObject.Credential);
        }        

        [When("I have provided the IPAFF Heathrow Inspector credentials and signin")]
        public void WhenIHaveProvidedTheIPAFFHeathrowInspectorCredentialsAndSignin()
        {
            var jsonData = UserObject?.GetUser("IPAFF", "Inspector-Heathrow");
            var userObject = new User
            {
                UserName = jsonData.UserName,
                Credential = jsonData.Credential
            };

            _signInPage?.SignIn(userObject.UserName, userObject.Credential);
        }

        [When("I have provided the IPAFF Gateway Inspector credentials and signin")]
        public void WhenIHaveProvidedTheIPAFFGatewayInspectorCredentialsAndSignin()
        {
            var jsonData = UserObject?.GetUser("IPAFF", "Inspector-Gateway");
            var userObject = new User
            {
                UserName = jsonData.UserName,
                Credential = jsonData.Credential
            };

            _signInPage?.SignIn(userObject.UserName, userObject.Credential);
        }

        [When(@"I have provided the password for prototype research page")]
        public void WhenIHaveProvidedThePasswordForPrototypeResearchPage()
        {
            _signInPage?.EnterPassword();
        }

        [Then("I click Sign in button")]
        public void ThenIClickSignInButton()
        {
            governmentGatewayTypePage?.ClickSignInButton();
        }
    }
}