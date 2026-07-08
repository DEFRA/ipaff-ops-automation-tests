namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IConfirmationOfCommodityRateChangePage
    {
        bool IsPageLoaded();
        IDictionary<string, string> GetConfirmationDetails();
        void ClickConfirmAndSendButton();
    }
}