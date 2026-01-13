namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IEnterBorderNotificationDetailsPage
    {
        bool IsPageLoaded();

        void SelectNotificationType(string type);
        void SelectNotificationBasis(string basis);
        void SelectProductCategory(string category);
        void EnterProductName(string name);
        void EnterBrandName(string brand);
        void EnterOtherLabelling(string label);
        void EnterOtherInformation(string otherInfo);
        string SelectDurabilityDateOption(string dateOption);
        void SelectRiskDecision(string decision);
        void SelectImpactOn(string impact);
        void SelectHazardCategory(string hazard);
        void SelectMeasureTaken(string measure);

        void SelectNotificationDetails(string type, string basis);
        void SelectOtherDetails(string label, string otherInfo, string dateOption);
        void SelectProductDetails(string category, string product, string brand);
        void SelectRiskDetails(string decision, string impact, string hazard, string measure);
    }
}
