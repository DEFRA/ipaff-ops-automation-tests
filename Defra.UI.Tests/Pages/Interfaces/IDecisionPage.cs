using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IDecisionPage
    { 
        bool IsPageLoaded();
        bool VerifyPrepopulatedTransitDetails(string exitBCP, string transitedCountry, string destinationCountry);
        bool VerifyTransitRadioButtonPrePopulated();
    }
}
