namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IDecisionPage
    {
        bool IsPageLoaded();
        bool IsAcceptableForRadioSelected(string acceptableForRadioOption);
        bool IsInternalMarketSubRadioSelected(string internalMarketSubOption);
        void SelectDecision(string subOption, string decision);
        bool VerifyPrepopulatedTransitDetails(string exitBCP, string transitedCountry, string destinationCountry);
        bool VerifyTransitRadioButtonPrePopulated();
        void SelectAcceptableFor(string acceptableFor, string subOption);
        void EnterReason(string reason);
        void SelectNotAcceptableFor(string acceptableFor, string subOption);
        bool IsRadioButtonPreSelected(string radioButtonName);
        string GetExitDate();
        string GetExitBCP();
        string GetDestinationCountry();
        string GetTransitExitBCP();
        void EnterDestructionReason(string reason);
        void SelectFutureDateFromDatePicker();
        void EnterDateInDecisionPage(string day, string month, string year);
    }
}