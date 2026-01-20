using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISearchExistingConsigneePage
    {
        bool IsPageLoaded();
        string GetSelectedConsignee(string consigneeName);
        void ClickSelect(string consigneeName);
        string GetSelectedConsigneeName(string consigneeName);
        string GetSelectedConsigneeAddress(string consigneeName);
        string GetSelectedConsigneeCountry(string consigneeName);
    }
}
