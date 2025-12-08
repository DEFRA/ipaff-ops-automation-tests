using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ITransportContactsPage
    { 
        bool IsPageLoaded();
        void SelectTransportContactNotification(string option);
    }
}
