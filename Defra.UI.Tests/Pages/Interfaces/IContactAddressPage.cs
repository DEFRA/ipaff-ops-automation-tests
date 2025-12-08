using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IContactAddressPage
    {
        bool IsPageLoaded();
        bool IsPageLoadedWithoutSecondaryTitle();
        string GetSelectedContactAddress();
    }
}
