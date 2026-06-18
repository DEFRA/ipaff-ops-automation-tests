namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IThereAreRulesInYourCSVFileThatAlreadyExistPage
    {
        bool IsPageLoaded();
        void SelectYesAndClickContinue();
        void SelectYes();
        void ClickContinue();
    }
}