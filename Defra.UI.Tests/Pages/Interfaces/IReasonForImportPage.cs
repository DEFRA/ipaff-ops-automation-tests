using Microsoft.VisualBasic.FileIO;
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
        void SelectTranshipmentDestination(string transhipmentCountry);
        void EnterExitDate(int daysFromToday);
        void SelectExitBCP(string exitBCP);
    }
}