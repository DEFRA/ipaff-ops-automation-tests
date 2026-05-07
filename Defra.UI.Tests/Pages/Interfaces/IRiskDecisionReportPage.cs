namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IRiskDecisionReportPage
    {
        bool IsPageLoaded();
        void Search(string chedReference);
        int GetRecordCount();
        void ClickExpandForCHED(string chedReference);
        void ClickRequestsDetails();
        void ClickDecisionDetails();
        string GetRequestsJson();
        string GetDecisionJson();
    }
}