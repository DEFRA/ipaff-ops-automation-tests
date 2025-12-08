using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IDeclarationPage
    { 
        bool IsPageLoaded();
        void ClickSubmitNotification();
    }
}
