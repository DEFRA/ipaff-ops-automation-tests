using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class EmailSignUpPage : IEmailSignUpPage
    {

        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;


        #region Page Objects
        private IWebElement PageHeading => _driver.WaitForElement(By.XPath("//h1[@class='govuk-heading-xl'] | //h1[@class='govuk-heading-l'] | //h1[@class='govuk-fieldset__heading']"), true);
        private IWebElement btnContinue => _driver.WaitForElement(By.XPath("//button[@id='button-continue']|//button[@id='continue']|//*[normalize-space(text())='Continue']"));
        private IWebElement txtEmailAddressPage => _driver.WaitForElement(By.XPath("//h1[text() = 'Enter your email address']"));
        private IWebElement txtboxEmail => _driver.WaitForElement(By.XPath("//input[@id='emailAddress']"),true);
        private IWebElement txtConfirmEmail => _driver.WaitForElement(By.XPath("//h1[text()='Enter code to confirm your email address']"),true);
        private IWebElement txtboxConfirmCode => _driver.WaitForElement(By.XPath("//input[@id='code']"));
        private IWebElement lblFullname => _driver.WaitForElement(By.XPath("//label[text()=' What is your full name? ']"), true);
        private IWebElement txtboxName => _driver.WaitForElement(By.XPath("//input[@id='name']"));
        private IWebElement lblPassword => _driver.WaitForElement(By.XPath("//h1[text()='Create a password']"), true);
        private IWebElement txtboxPassword => _driver.WaitForElement(By.XPath("//input[@id='newPassword']"));
        private IWebElement txtboxConfirmPassword => _driver.WaitForElement(By.XPath("//input[@id='confirmPassword']"));
        private IWebElement lblGGID => _driver.WaitForElement(By.XPath("//h1[text()='Your Government Gateway user ID is:']//following-sibling::div"), true);
        private IWebElement optIndividualUser => _driver.WaitForElement(By.XPath("//input[@id='accountType-2']//following-sibling::label"), true);
        private IWebElement txtboxFirstName => _driver.WaitForElement(By.XPath("//input[@id='firstName']"), true);
        private IWebElement txtboxLastName => _driver.WaitForElement(By.XPath("//input[@id='lastName']"));
        private IWebElement txtboxTelephoneNumber => _driver.WaitForElement(By.XPath("//input[@id='telephoneNumber']"), true);
        private IWebElement txtboxPostcode => _driver.WaitForElement(By.XPath("//input[@id='postcode']"), true);
        private IWebElement DrpdownSelectAddress => _driver.WaitForElement(By.XPath("//select[@id='address']"));
        private IWebElement txtboxSecurityWord => _driver.WaitForElement(By.XPath("//input[@id='securityWord']"),true);
        private IWebElement txtAreaSecurityHint => _driver.WaitForElement(By.XPath("//textarea[@id='securityHint']"));

        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public EmailSignUpPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return PageHeading.Text.Contains("Enter your email address");
        } 
        
        public void EnterEmailAddress(String emailAddress)
        {
            txtboxEmail.SendKeys(emailAddress);
        }
        
        public void ClickContinueButton()
        {
            btnContinue.Click();
        }

        public void EnterConfirmationCode(string confirmationCode)
        {
            txtConfirmEmail.Text.Contains("Enter code to confirm your email address");
            txtboxConfirmCode.SendKeys(confirmationCode);
        }

        public void EnterFullName(string Name)
        {
            lblFullname.Text.Contains("What is your full name?");
            txtboxName.SendKeys(Name);
        }
        
        public void EnterThePassword(string Password)
        {
            lblPassword.Text.Contains("Create a password");
            txtboxPassword.SendKeys(Password);
            txtboxConfirmPassword.SendKeys(Password);
        }

        public string GetGGID()
        {
            return lblGGID.Text;
        }

        public void SelectIndividualUser()
        {
            optIndividualUser.Click();
        }

        public void EnterFirstAndLastName(string firstName, string lastName)
        {
            txtboxFirstName.SendKeys(firstName);
            txtboxLastName.SendKeys(lastName);
        }
        
        public void EnterTelephoneNumber(string telephoneNumber)
        {
            txtboxTelephoneNumber.SendKeys(telephoneNumber);
        }
        
        public void EnterPostCode(string postCode)
        {
            txtboxPostcode.SendKeys(postCode);
        }
        
        public void SelectAddress()
        {
           SelectElement select = new SelectElement(DrpdownSelectAddress);
           select.SelectByIndex(1);
        }
        
        public void EnterMemorableWordAndHint(string securityWord, string hint)
        {
            txtboxSecurityWord.SendKeys(securityWord);
            txtAreaSecurityHint.SendKeys(hint);
        }

    }
}
