using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IReasonForImportPage
    {
        bool IsPageLoaded();
        bool AreImportReasonsPresent();
        bool AreImportReasonsForCHEDDPresent();
        bool IsElementPresent(IWebElement element);
        void SelectReasonForImport(string option);
        void SelectReasonForImportSubOption(string subOption);
        bool IsReasonForImportingAnimalsPageLoaded();
        bool AreImportAnimalsReasonsPresent();
        void SelectExitBorderControlPost(string exitBCP);
        void EnterConsignmentLeavingDate(string day, string month, string year);
        void EnterConsignmentLeavingTime(string hours, string minutes);
        void SelectTransitedCountry(string transitedCountry);
        void SelectDestinationCountry(string destinationCountry);
        void SelectDestinationCountryBasedOnContext(string destinationCountry);
        void SelectTranshipmentDestination(string transhipmentCountry);
        string EnterConsignmentDepartureDate();
        string EnterConsignmentDepartureTime();
        void AddPlaceOfExit(string placeOfExit);
        void EnterExitDate(int daysFromToday);
        void SelectExitBCP(string exitBCP);
        void SelectExitBCPBasedOnContext(string exitBCP);
        bool VerifyInternalMarketHasSubOptions(int expectedCount);
        bool VerifyTranshipmentHasDestinationCountryDropdown();
        bool VerifyTransitHasExitBCPAndDestinationDropdowns();
        bool VerifyReentryHasNoSubOptions();
        bool VerifyTemporaryAdmissionHasExitDateAndBCPDropdown();
        string GetReasonForImportRadioLabelText { get; }
    }
}