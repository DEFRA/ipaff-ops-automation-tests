namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IChooseOperatorTypePage
    {
        bool IsPageLoaded();
        void SelectOperatorType(string operatorType);
        bool AreRadioButtonsDisplayed(string radioButton1, string radioButton2, string radioButton3, string radioButton4);
        void ClickContinue();
    }
}