using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IReasonForRefusalPage
    {
        bool IsPageLoaded();
        void SelectReasonForRefusal(params string[] reason);
    }
}
