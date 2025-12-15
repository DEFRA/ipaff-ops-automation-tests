namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IChecksPage
    {
        bool IsChecksPageLoaded();
        void SelectDocCheckRadio(string docCheckOption);
        void SelectIdentityCheckRadio(string identityCheckOption);
        void SelectIdentityCheckSubRadio(string identityCheckSubOption);
        void SelectPhysicalCheckRadio(string physicalCheckOption);
        void SelectPhysicalCheckSubRadio(string physicalCheckSubOption);
        void ClickSaveAndContinueButton();
    }
}