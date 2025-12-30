namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IEnterBorderNotificationDetailsPage
    {
        bool IsPageLoaded();
        void SelectNotificationDetails(string type, string basis);
        void SelectOtherDetails(string label, string otherInfo, string dateOption);
        void SelectProductDetails(string category, string product, string brand);
        void SelectRiskDetails(string decision, string impact, string hazard, string measure);
    }
}
