namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IConsignmentsRequiringControlPage
    {
        bool IsPageLoaded();
        bool VerifyNotificationStatus(string chedRef, string status);
    }
}
