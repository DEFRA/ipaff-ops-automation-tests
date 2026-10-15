namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterYourApplicationPage
    {
        bool IsPageLoaded();
        string GetTaskStatus(string taskName);
        void ClickWhatsInYourConsignmentLink();
        void ClickWhatAreTheInspectionDetailsLink();
        void ClickHowWillThisConsignmentBeTransportedLink();
        void ClickWhatAreThePackerDetailsLink();
        void ClickCheckYourAnswersAndSubmitYourApplicationLink();
        void ClickWhatIsTheExporterAddressLink();
        void ClickWhoAreYouSendingThisConsignmentToLink();
        void ClickSupportingDocumentsLink();
    }
}