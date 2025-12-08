using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISearchExistingTranspoterPage
    {
        bool IsPageLoaded();
        void ClickSelect();
        string GetSelectedTransporterName();
        string GetSelectedTransporterAddress();
        string GetSelectedTransporterCountry();
        string GetSelectedTransporterApprovalNumber();
        string GetSelectedTransporterType();
    }
}
