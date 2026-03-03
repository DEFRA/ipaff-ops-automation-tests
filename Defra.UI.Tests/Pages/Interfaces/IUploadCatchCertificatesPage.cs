namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IUploadCatchCertificatesPage
    {
        bool IsPageLoaded(string operatorName);
        bool VerifySelectDocumentsHeading();
        bool VerifyDragAndDropFunctionality();
        void UploadMultipleCatchCertificates(params string[] fileNames);
        bool VerifyFilesAreDisplayed(int expectedFileCount, params string[] expectedFileNames);
        void ClickContinue();
    }
}