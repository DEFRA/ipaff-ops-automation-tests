using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISearchExistingDestinationPage
    {
        bool IsPageLoaded();
        void ClickSelect(string destination);
        string GetSelectedPlaceOfDestination(string destination);
        string GetSelectedDestinationName(string destination);
        string GetSelectedDestinationAddress(string destination);
        string GetSelectedDestinationCountry(string destination);
    }
}
