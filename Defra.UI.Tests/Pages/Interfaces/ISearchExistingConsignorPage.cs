using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISearchExistingConsignorPage
    {
        bool IsPageLoaded();
        void ClickSelect();
        string GetSelectedConsignor();
        string GetSelectedConsignorName();
        string GetSelectedConsignorAddress();
        string GetSelectedConsignorCountry();
    }
}
