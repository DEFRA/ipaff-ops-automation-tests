namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterReviewYourAnswersPage
    {
        bool IsPageLoaded();
        void ClickContinueButton();
        string GetCommodityTableText();
        string GetConsignmentValue();
        string GetPointOfEntry();
        string GetTransportMode();
        string GetDestinationCountry();
        string GetYourReference();
        string GetCommodityDescription();
        string GetCommodityVariety();
        string GetCommodityLotReference();
        string GetCommodityOrigin();
        string GetCommodityQuantity();
        string GetCommodityNumberOfPackages();
        string GetCommodityPackaging();
        string GetConsigneeName();
        string GetConsigneeAddress();
        void ClickChangeLink(string fieldName);
    }
}