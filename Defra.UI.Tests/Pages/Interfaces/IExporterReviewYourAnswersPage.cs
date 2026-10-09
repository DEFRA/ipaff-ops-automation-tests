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
    }
}