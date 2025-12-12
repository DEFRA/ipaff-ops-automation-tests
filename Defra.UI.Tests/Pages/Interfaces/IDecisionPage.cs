using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IDecisionPage
    {
        void EnterCurrentDateInDecisionPage(string day,string month,string year);
        bool IsPageLoaded();
        bool IsAcceptableForRadioSelected(string acceptableForRadioOption);
        bool IsInternalMarketSubRadioSelected(string internalMarketSubOption);
        void SelectDecision(string subOption, string decision);
        bool VerifyPrepopulatedTransitDetails(string exitBCP, string transitedCountry, string destinationCountry);
        bool VerifyTransitRadioButtonPrePopulated();
        void SelectAcceptableFor(string acceptableFor, string subOption);
    }
}