using Defra.UI.Tests.Tools;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IAddOperatorDetailsPage
    {
        bool IsPageLoaded();
        void EnterOperatorName(string name);
        void EnterAddressLine1(string addressLine1);
        void EnterCityOrTown(string cityOrTown);
        void EnterPostcode(string postcode);
        void SelectCountry(string country);
        void EnterTelephoneNumber(string telephoneNumber);
        void EnterEmail(string email);
        void EnterOperatorDetails(OperatorDetails operatorDetails);
        void ClickSaveAndContinue();
    }
}