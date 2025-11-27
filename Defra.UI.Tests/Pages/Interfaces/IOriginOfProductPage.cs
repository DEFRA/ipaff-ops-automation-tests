using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IOriginOfProductPage
    {
        bool IsPageLoaded();
        void SelectCountryOfOrigin(string country);
    }
}
