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
        void SearchForTransporter(string transporterName);
        void ClickSelectForTransporter(string transporterName);
        string GetSelectedTransporterName(string transporterName);
        string GetSelectedTransporterAddress(string transporterName);
        string GetSelectedTransporterCountry(string transporterName);
        string GetSelectedTransporterApprovalNumber(string transporterName);
        string GetSelectedTransporterType(string transporterName);
    }
}
