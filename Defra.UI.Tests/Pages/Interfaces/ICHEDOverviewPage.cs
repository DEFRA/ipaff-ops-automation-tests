namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICHEDOverviewPage
    {
        void ClickCopyAsReplacement();
        void ClickRaiseBorderNotification();
        void ClickReplacedByLink();
        bool IsPageLoaded();
        bool VerifyCHEDReference(string type, string chedReference, string replacementChedReference);
        bool VerifyReplacedByLink(string type, string chedReference, string replacementChedReference);
    }
}
