namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IChooseApprovedEstablishmentPage
    {
        bool IsPageLoaded();
        void SelectCountryAndSearch(string country);
        void EnterEstablishmentName(string name);
        bool AreAllResultsForCountry(string country);
        void SelectEstablishmentByName(string name);
    }
}