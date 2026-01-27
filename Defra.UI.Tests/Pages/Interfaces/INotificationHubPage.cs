using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface INotificationHubPage
    {
        bool IsPageLoaded();
        void ClickCommodityLink();
        void ClickCountriesTheConsignmentWillTravelThroughLink();
        void ClickLink(string link);
        string GetNotificationVersion();
    }
}