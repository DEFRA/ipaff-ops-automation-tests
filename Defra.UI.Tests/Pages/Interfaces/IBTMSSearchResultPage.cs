namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IBTMSSearchResultPage
    {
        bool IsPageLoaded(string CHEDPREFNum);
        bool ValidateBTMSSearchResult(string commodityCode, string commodityDescription, string commodityQuantity, string authority, string decision);
        string GetCommodityCode(string commodityNum);
        string GetCommodityDesc(string commodityNum);
        string GetCommodityQuantity(string commodityNum);
        string GetCommodityAuthority(string commodityNum);
        string GetCommodityDecision(string commodityNum);
        bool VerifyStatus(string status);
        bool IsPageLoadedForReplacementCHED(string replacementCHEDPREFNum);
    }
}