namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IConfirmExemptSpeciesPage
    {
        bool IsPageLoaded(string pageTitle);
        void SelectSpeciesOption(string option);
    }
}