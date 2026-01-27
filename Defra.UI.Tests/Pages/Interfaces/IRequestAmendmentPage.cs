namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IRequestAmendmentPage
    {
        bool IsPageLoaded();
        void EnterAmendmentReason(string reason);
        void ClickRequestAmendmentButton();
        void ClickDoNotRequestAmendment();
        string GetCHEDReference();
        string GetStatus();
    }
}