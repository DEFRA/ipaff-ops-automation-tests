using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISearchExistingImporterPage
    {
        bool IsPageLoaded();
        void ClickSelect(string importer);
        string GetSelectedImporterName();
        string GetSelectedImporterAddress();
        string GetSelectedImporterCountry();
        string GetSelectedImporter();
    }
}
