using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface INotificationHubPage
    { 
        bool IsPageLoaded();
        string GetRefNumber {  get; }
        void ClickCommodityLink();
        void ClickContactAddressForConsignmentLink();
        void ClickCountriesTheConsignmentWillTravelThroughLink();
        void ClickLink(string link);
    }
}
