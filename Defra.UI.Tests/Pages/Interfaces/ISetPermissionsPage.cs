namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISetPermissionsPage
    {
        bool IsPageLoaded();
        void ToggleAllPermissionsToYes();
        void ClickFinish();
    }
}