namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IManageCatchCertificatesPage
    {
        bool IsPageLoaded(string pageTitle);
        void SelectOption(string option);
        bool VerifyNumberOfCertificates(string numberOfCertificates);
        void ClickAddDetailsLink();
        bool VerifySuccessMessageDisplaysNumberOfFilesAdded(int expectedFileCount);
        bool VerifyNumberOfAttachmentsMissingDetails(int expectedMissingCount);
        bool VerifyCatchCertificatesUploadedHeadingDisplayed();
        bool VerifyNumberOfCatchCertificatesDisplayedForEachAttachment(int expectedAttachmentCount);
        bool VerifyEachAttachmentDisplayedAsXofYFormat(int totalAttachments);
        bool VerifyEachAttachmentHasPopulatedInputBox(int expectedAttachmentCount);
        bool VerifyEachAttachmentHasUpdateButton(int expectedAttachmentCount);
        bool VerifyEachAttachmentHasAddDetailsAndRemoveLinks(int expectedAttachmentCount);
        void ClickViewOrAmendDetailsLinkForAttachment(int attachmentNumber);
    }
}