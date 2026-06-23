namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICHEDPPHMISystemConfigPage
    {
        bool IsPageLoaded();
        int? GetCurrentAISRate();
        void SetNewAISRate(int newAISRate);
        void ClickSaveAndContinueButton();
    }
}
