using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IApprovedEstablishmentPage
    {
        bool IsPageLoaded();
        void ClickSearchForApproved();
        bool VerifySelectedCountryOfOrigin(string country);
        void ClickSelectEstablishment();
        bool VerifySelectedEstablismentName(string establishmentListFirstName);
        string GetSubtotalNetWeight();
        string GetSubtotalPackages();
        string GetTotalNetWeight();
        string GetTotalPackages();
        string GetEstablishmentListFirstName();
    }
}
