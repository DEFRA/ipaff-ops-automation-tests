namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IWhichImportPurposesWillThisCommodityRuleApplyToPage
    {
        bool IsPageLoaded();
        void TickBothImportPurposeCheckboxes();
        void ClickContinueButton();
    }
}