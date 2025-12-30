namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IConsignmentsRequiringControlPage
    {
        void ClickCHEDReferencNum();
        bool IsPageLoaded();
        bool VerifyNotificationStatus(string chedRef, string status);
    }
}
