namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICHEDOverviewPage
    {
        void ClickClearAll();
        void ClickCopyAsReplacement();
        void ClickRaiseBorderNotification();
        void ClickRecordControl();
        void ClickReplacedByLink();
        bool IsFieldValuePresent(string fieldName);
        bool IsFieldValuePresent(string fieldName, string sectionName);
        bool IsFieldValuePresentInTable(string fieldName, string column);
        bool IsPageLoaded();
        void SwitchTab(string tabName);
        bool VerifyCHEDReference(string type, string chedReference, string replacementChedReference);
        bool VerifyDecisionRecordedBy(string fieldName, string status);
        bool VerifyDocumentCheck(string status);
        bool VerifyReplacedByLink(string type, string chedReference, string replacementChedReference);
        bool VerifyRiskDecisionHMI(string decision);
        bool VerifyRiskDecisionPHSI(string decision);
        bool VerifyShowChedButton();
        bool VerifyTab(string tabName);
        void ClickRecordControlButton();
    }
}