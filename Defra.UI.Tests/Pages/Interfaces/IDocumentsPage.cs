namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IDocumentsPage
    {
        bool IsPageLoaded();
        void ClickSaveAndContinue();
        bool IsAddAnotherDocumentLinkDisplayed();
        bool VerifyNoDocumentsInInspectorSection();
        (string? documentType, string? documentReference, string? dateOfIssue) GetInspectorDocumentDetails(int index = 0);
        void ClickDownloadAllDocumentsLink();
        void ClickDownloadUrlLink();
        bool IsSingleCertificagteDownloaded(string chedReference);
        bool IsCatchCertificateSummaryUrlDisplayed();

        void ClickDownloadLinkInCatchCertificate();
        bool IsCatchCertificateDownloaded(string catchCert);

    }
}