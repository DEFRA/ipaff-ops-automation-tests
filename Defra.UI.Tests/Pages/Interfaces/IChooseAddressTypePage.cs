namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IChooseAddressTypePage
    {
        bool IsPageLoaded();
        void SelectAddressType(string addressType);
        bool AreRadioButtonsDisplayed(string radioButton1, string radioButton2);
        void ClickContinue();
    }
}