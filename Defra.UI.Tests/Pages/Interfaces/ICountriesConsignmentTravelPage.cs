using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICountriesConsignmentTravelPage
    { 
        bool IsPageLoaded();
        void SelectCountry(string country);
        void ClickAddAnotherCountry();
    }
}
