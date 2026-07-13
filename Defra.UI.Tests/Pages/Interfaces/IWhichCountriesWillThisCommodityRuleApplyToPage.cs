namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IWhichCountriesWillThisCommodityRuleApplyToPage
    {
        bool IsPageLoaded();
        void SelectCountry(string country);
        void ClickContinueButton();
    }
}