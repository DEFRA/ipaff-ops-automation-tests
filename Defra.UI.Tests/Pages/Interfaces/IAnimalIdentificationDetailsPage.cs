using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IAnimalIdentificationDetailsPage
    {
        bool IsPageLoaded();
        void EnterIdentificationDetails(string identificationDetails);
        void EnterDescription(string description);
    }
}
