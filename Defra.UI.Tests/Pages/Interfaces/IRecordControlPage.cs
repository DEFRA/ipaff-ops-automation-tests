namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IRecordControlPage
    {
        bool IsPageLoaded();
        void SelectConsignmentLeaveRadio(string consignmentLeaveDecision);
        void SelectMeansOfTransport(string transport);
        void EnterIdentification(string identification);
        void EnterDocumentation(string identification);
        void SelectDateFromDatePicker();
        void SelectExitBCP(string exitBCP);
        void SelectDestinationCountry(string destination);
        void ClickSubmitControlButton();
        void EnterDate(string day, string month, string year);
    }
}