namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IPhytosanitaryCertificateDetailsPage
    {
        bool IsPageLoaded();
        bool VerifyContentAndTitlesOnPage();
        Dictionary<string, string> GetKeyAndValuesOfSummaryAndGoods();
        bool IsCloneAndCancelButtonExists();
        void ClickCloneButton();
    }
}
