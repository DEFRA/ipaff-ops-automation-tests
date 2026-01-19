namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISelectTheTransporterTypePage
    {
        bool IsPageLoaded();
        void SelectTransporterType(string transporterType);
        void ClickSaveAndContinue();
    }
}