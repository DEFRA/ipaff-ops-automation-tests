using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IBTMSSearchPage
    {
        bool IsPageLoaded();
        void SearchForChed(string chedRef);
    }
}
