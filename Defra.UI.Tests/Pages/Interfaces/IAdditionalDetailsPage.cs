using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IAdditionalDetailsPage
    {
        bool IsPageLoaded();
        bool IsAdditionalAnimalDetailsPageLoaded();
        void ClickImportingProduct(string option);
        void SelectAnimalCertification(string certificationOption);
        void SelectUnweanedAnimalsOption(string option);
    }
}
