namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IOverrideRiskDecisionPage
    {
        bool IsPageLoaded();
        void ClickYesOverrideRiskDecisionButton();
        void ClickOverrideDecisionOption(string option);
    }
}
