using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISearchExistingImporterPage
    {
        bool IsPageLoaded();
        void ClickSelect(string importer);
        string GetSelectedImporterName(string importerName);
        string GetSelectedImporterAddress(string importerName);
        string GetSelectedImporterCountry(string importerName);
        string GetSelectedImporter(string importerName);
    }
}
