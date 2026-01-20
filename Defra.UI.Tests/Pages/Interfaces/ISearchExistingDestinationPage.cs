namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISearchExistingDestinationPage
    {
        bool IsPageLoaded();
        void ClickSelect(string destinationName);
        string GetSelectedPlaceOfDestination(string destinationName);
        string GetSelectedDestinationName(string destinationName);
        string GetSelectedDestinationAddress(string destinationName);
        string GetSelectedDestinationCountry(string destinationName);
    }
}