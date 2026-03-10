namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IChangeOrganisationSettingsPage
    {
        bool IsPageLoaded();
        bool IsConfirmationPageLoaded(string expectedMessage);
        bool IsInstructionTextDisplayed();
        bool IsSelectAllThatApplyHintDisplayed();
        bool IsAuthoriseAgentCheckboxDisplayed();
        bool IsActAsAgentCheckboxDisplayed();
        bool IsAgentDeclarationTextDisplayed();
        bool IsConfirmationCheckboxDisplayed();
        bool IsWarningMessageDisplayed();
        bool IsSaveButtonDisplayed();
        void TickAuthoriseAgentCheckbox();
        void UntickActAsAgentCheckbox();
        void TickConfirmationCheckbox();
        void ClickSave();
        void ClickContinue();
    }
}