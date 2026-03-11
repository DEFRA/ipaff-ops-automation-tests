using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface INotificationOverviewPage
    {
        void ClickChangeInCommoditySection();
        void ClickRecordChecksButton();
        void ClickSetToInProgressButton();
        string GetCHEDReference();
        string GetCustomsDeclarationReference();
        string GetCustomsDocumentCode();
        bool IsPageLoaded();
        bool VerifyStatus(string status);
        bool VerifyTotalGrossWeight(string grossWeight);
        bool VerifyTotalNetWeight(string netWeight);
        void ClickRequestAmendment();
        bool StatusContains(string status);
    }
}
