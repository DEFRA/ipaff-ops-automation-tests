namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterDestinationCountryPage
    {
        bool IsPageLoaded();
        void SearchAndSelectCountry(string country);
        void ClickContinueButton();
    }
}