using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IDecisionPage
    { 
        bool IsPageLoaded();
        bool IsAcceptableForRadioSelected(string acceptableForRadioOption);
        bool IsInternalMarketSubRadioSelected(string internalMarketSubOption);
    }
}
