namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISetTheInspectionRatePage
    {
        bool IsPageLoaded();
        void EnterInspectionRate(string rate);
        void ClickContinueButton();
    }
}