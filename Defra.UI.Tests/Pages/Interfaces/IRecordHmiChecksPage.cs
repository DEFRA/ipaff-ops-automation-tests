namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IRecordHmiChecksPage
    {
        bool IsPageLoaded();
        bool VerifyCommodityHmiStatus(string expectedStatus);
        void SetAllCommoditiesStatus(string status);
        void SetValidityPeriod(int days);
        void ClickSaveAndReturnToWorkOrder();
    }
}