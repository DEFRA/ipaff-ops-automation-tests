using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class ChooseOperatorTypeSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        private IChooseOperatorTypePage? chooseOperatorTypePage => _objectContainer.IsRegistered<IChooseOperatorTypePage>() ? _objectContainer.Resolve<IChooseOperatorTypePage>() : null;

        public ChooseOperatorTypeSteps(ScenarioContext context, IObjectContainer container)
        {
            _objectContainer = container;
            _scenarioContext = context;
        }

        [Then(@"the Choose operator type page should be displayed with '([^']*)' '([^']*)' '([^']*)' and '([^']*)' radio buttons")]
        public void ThenTheChooseOperatorTypePageShouldBeDisplayedWithRadioButtons(string radioButton1, string radioButton2, string radioButton3, string radioButton4)
        {
            Assert.True(chooseOperatorTypePage?.IsPageLoaded(), "Choose Operator Type page not loaded");
            Assert.True(chooseOperatorTypePage?.AreRadioButtonsDisplayed(radioButton1, radioButton2, radioButton3, radioButton4),
                $"Radio buttons '{radioButton1}', '{radioButton2}', '{radioButton3}', and '{radioButton4}' are not displayed");
        }

        [When(@"the user selects operator type '([^']*)' and clicks Continue")]
        public void WhenTheUserSelectsOperatorTypeAndClicksContinue(string operatorType)
        {
            chooseOperatorTypePage?.SelectOperatorType(operatorType);
            chooseOperatorTypePage?.ClickContinue();
        }
    }
}