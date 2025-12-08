using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IControlledDestinationPage
    {
        bool IsPageLoaded();
        void ClickAddControlledDestination();
        bool VerifySelectedControlledDestination(string name, string address, string type);
        void ClickSaveAndContinue();
    }
}