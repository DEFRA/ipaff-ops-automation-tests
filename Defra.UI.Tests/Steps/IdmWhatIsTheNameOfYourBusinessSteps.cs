using Defra.UI.Tests.Tools;
using Defra.UI.Tests.Pages.Interfaces;
using NUnit.Framework;
using Reqnroll;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Steps.IPAFF
{
    [Binding]
    public class IdmWhatIsTheNameOfYourBusinessSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private IIdmWhatIsTheNameOfYourBusinessPage idmWhatIsTheNameOfYourBusinessPage => _objectContainer.Resolve<IIdmWhatIsTheNameOfYourBusinessPage>();
        public IdmWhatIsTheNameOfYourBusinessSteps(ScenarioContext context, IObjectContainer container)
        {
            _scenarioContext = context;
            _objectContainer = container;
        }

        [Then("the What is the name of your business? page is displayed")]
        public void ThenPageIsDisplayed() => Assert.That(idmWhatIsTheNameOfYourBusinessPage.IsPageLoaded(), Is.True);

        [When("the user enters a unique business name and continues")]
        public void WhenBusinessNameIsEntered()
        {
            var businessName = Utils.GenerateOperatorDetails("Importer").OperatorName;
            _scenarioContext.Set(businessName, "RegistrationBusinessName");
            idmWhatIsTheNameOfYourBusinessPage.EnterBusinessNameAndContinue(businessName);
        }
    }
}
