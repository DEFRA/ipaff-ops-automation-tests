namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IPortOfEntryPage
    {
        bool IsPageLoaded();
        bool IsBCPOrPortOfEntryPageLoaded();
        void EnterPortOfEntry(string port);
        void SelectMeansOfTransport(string mode);
        void EnterTransportId(string transId);
        void EnterTransportDocRef(string DocumentRef);
        void EnterEstimatedArrivalDate(string day, string month, string year);
        void EnterEstimatedArrivalTime(string hour, string minutes);
        void EnterEstimatedJourneyTime(string hours);
        void EnterContainerDetails(string containerNumber, string sealNumber);
        void SelectAreTrailersOrContainersUsed(string option);
        void EnterContainerNumber(string containerNumber);
        void EnterSealNumber(string sealNumber);
        void TickOfficialSealCheckbox();
        bool VerifyPortOfEntryIfNotAlreadyPopulated();
    }
}