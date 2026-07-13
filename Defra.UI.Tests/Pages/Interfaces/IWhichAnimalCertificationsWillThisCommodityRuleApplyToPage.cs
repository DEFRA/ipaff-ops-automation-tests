namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IWhichAnimalCertificationsWillThisCommodityRuleApplyToPage
    {
        bool IsPageLoaded();
        void TickCheckbox(string checkboxLabel);
        void ClickContinueButton();
    }
}