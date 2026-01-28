using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IReasonForRefusalPage
    {
        void EnterReasonTextForOther(string reasonText);
        bool IsPageLoaded();
        void SelectReasonForRefusal(params string[] reason);
    }
}
