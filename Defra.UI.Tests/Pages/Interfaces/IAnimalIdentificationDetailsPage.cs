using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IAnimalIdentificationDetailsPage
    {
        bool IsPageLoaded();
        void EnterIdentificationDetails(string identificationDetails);
        void EnterDescription(string description);
        void EnterHorseName(string horseName);
        void EnterMicrochipNumber(string microchipNumber);
        void EnterPassportNumber(string passportNumber);
        void EnterEarTag(string earTag);
        string GetEarTag { get; }
        string GetNumberOfAnimals();
    }
}