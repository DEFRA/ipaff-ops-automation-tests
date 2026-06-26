namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterDoYouWantToSelectThisInspectionAddressPage
    {
        bool IsPageLoaded();
        void SelectInspectionAddressOptionAndContinue(string option);
    }
}