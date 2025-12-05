using Microsoft.VisualBasic.FileIO;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IReasonForImportPage
    { 
        bool IsPageLoaded();
        bool AreImportReasonsPresent();
        bool AreImportReasonsForCHEDDPreset();
        bool IsElementPresent(IWebElement element);
        void SelectReasonForImport(string option);
        void SelectReasonForImportSubOption(string subOption);
    }
}