namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IReviewBorderNotificationPage
    {
        bool IsPageLoaded();
        string GetCommodity { get; }
        string GetNetWeight { get; }
        string GetLaboratoryTest { get; }
        string GetCountry { get; }
        string GetNotificationType { get; }
        string GetNotificationBasis { get; }
        string GetProductCategory { get; }
        string GetProductName { get; }
        string GetBrandName { get; }
        string GetOtherLabelling { get; }
        string GetOtherInformation { get; }
        string GetDurabilityDate { get; }
        string GetRiskDecision { get; }
        string GetImpactOn { get; }
        string GetHazardCategory { get; }
        string GetMeasureTaken { get; }
        string GetAccompanyingDocumentType { get; }
        string GetAccompanyingDocumentRef { get; }
        string GetAccompanyingDocumentFileName { get; }
        string GetLastUpdatedDate { get; }
        string GetLastUpdatedTime { get; }
    }
}