using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISearchExistingControlledDestinationPage
    {
        bool IsPageLoaded();
        void ClickSelect();
        string GetSelectedControlledDestinationName();
        string GetSelectedControlledDestinationAddress();
        string GetSelectedControlledDestinationType();
        string GetSelectedControlledDestinationApprovalNumber();
    }
}