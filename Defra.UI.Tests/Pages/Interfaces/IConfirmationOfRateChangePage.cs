namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IConfirmationOfRateChangePage
    {
        bool IsPageLoaded();
        IDictionary<string, string> GetConfirmationDetails();
        void ClickConfirmAndSendButton();
    }
}