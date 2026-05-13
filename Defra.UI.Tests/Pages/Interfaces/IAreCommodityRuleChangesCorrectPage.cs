namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IAreCommodityRuleChangesCorrectPage
    {
        bool IsPageLoaded();
        IDictionary<string, string> GetSummaryDetails();
        void SelectConfirmChangesOption(string optionLabel);
        void ClickSubmitButton();
    }
}