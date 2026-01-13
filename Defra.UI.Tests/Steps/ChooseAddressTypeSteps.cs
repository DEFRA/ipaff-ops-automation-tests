using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ChooseAddressTypeSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IWebDriver? _driver => _objectContainer.IsRegistered<IWebDriver>() ? _objectContainer.Resolve<IWebDriver>() : null;
        private IChooseAddressTypePage? chooseAddressTypePage => _objectContainer.IsRegistered<IChooseAddressTypePage>() ? _objectContainer.Resolve<IChooseAddressTypePage>() : null;

        public ChooseAddressTypeSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then(@"the Choose address type page should be displayed with '([^']*)' and '([^']*)' radio buttons")]
        public void ThenTheChooseAddressTypePageShouldBeDisplayedWithRadioButtons(string radioButton1, string radioButton2)
        {
            Assert.True(chooseAddressTypePage?.IsPageLoaded(), "Choose address type page not loaded");
            Assert.True(chooseAddressTypePage?.AreRadioButtonsDisplayed(radioButton1, radioButton2),
                $"Radio buttons '{radioButton1}' and '{radioButton2}' are not displayed");
        }

        [When(@"the user selects address type '([^']*)' and clicks Continue")]
        public void WhenTheUserSelectsAddressTypeAndClicksContinue(string addressType)
        {
            chooseAddressTypePage?.SelectAddressType(addressType);
            chooseAddressTypePage?.ClickContinue();
        }
    }
}