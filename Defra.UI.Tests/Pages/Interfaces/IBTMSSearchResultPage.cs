using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IBTMSSearchResultPage
    {
        bool IsPageLoaded();
        bool ValidateBTMSSearchResult(string commodityCode, string commodityDescription, string commodityQuantity, string authority, string decision);
    }
}
