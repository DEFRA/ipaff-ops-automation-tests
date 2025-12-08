using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICountyParishHoldingPage
    {
        bool IsPageLoaded();
        void EnterCPHNumber(string cphNumber);
    }
}
