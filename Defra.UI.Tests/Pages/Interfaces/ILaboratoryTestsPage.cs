using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ILaboratoryTestsPage
    { 
        bool IsPageLoaded();
        void SelectLabTestsRadio(string labTestsOption);
    }
}
