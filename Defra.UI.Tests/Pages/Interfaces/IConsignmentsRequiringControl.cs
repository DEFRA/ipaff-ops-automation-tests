namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IConsignmentsRequiringControlPage
    {
        void ClickCHEDReferencNum();
        bool IsPageLoaded();
        bool VerifyControlStatus(string controlStatus);
        bool VerifyNotificationStatus(string chedRef, string status);
    }
}
