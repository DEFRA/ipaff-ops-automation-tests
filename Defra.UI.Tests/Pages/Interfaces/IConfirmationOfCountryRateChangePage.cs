namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IConfirmationOfCountryRateChangePage
    {
        bool IsPageLoaded();
        IDictionary<string, string> GetConfirmationDetails();
        void ClickConfirmAndSendButton();
    }
}