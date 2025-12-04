using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IChecksSubmittedPage
    {
        bool IsPageLoaded();
        string GetCHEDReferenceWithVersion();
        string GetOutcome();
    }
}
