using Defra.UI.Tests.Tools;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IPermanentAddressesPage
    {
        bool IsPageLoaded();
        void SelectAddressOptionForAllAnimals(string option);
        void EnterPermanentAddressDetails(int animalIndex);
        OperatorDetails EnterPermanentAddressForAnimal(string species, int animalIndex);
        void ClickSaveAndContinue();
        void ClickSaveAndReturnToHub();
    }
}