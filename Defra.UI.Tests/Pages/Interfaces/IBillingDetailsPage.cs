using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IBillingDetailsPage
    {
        void ClickSaveAndContinue();
        bool IsPageLoaded();
    }
}
