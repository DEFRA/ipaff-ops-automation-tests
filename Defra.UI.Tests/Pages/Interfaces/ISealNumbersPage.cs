using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISealNumbersPage
    {
        bool IsPageLoaded();
        bool IsSealNumbersNoPreselected();
    }
}