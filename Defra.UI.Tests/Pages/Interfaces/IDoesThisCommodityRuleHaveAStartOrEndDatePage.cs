namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IDoesThisCommodityRuleHaveAStartOrEndDatePage
    {
        bool IsPageLoaded();
        void SelectRadioButton(string option);
        void ClickContinueButton();
    }
}