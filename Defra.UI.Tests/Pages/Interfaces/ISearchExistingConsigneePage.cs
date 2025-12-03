using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISearchExistingConsigneePage
    {
        bool IsPageLoaded();
        void ClickSelect();
        string GetSelectedConsignee();
    }
}
