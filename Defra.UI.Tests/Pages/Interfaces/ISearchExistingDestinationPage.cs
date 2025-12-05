using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISearchExistingDestinationPage
    {
        bool IsPageLoaded();
        void ClickSelect();
        string GetSelectedPlaceOfDestination();
        string GetSelectedDestinationName();
        string GetSelectedDestinationAddress();
        string GetSelectedDestinationCountry();
    }
}
