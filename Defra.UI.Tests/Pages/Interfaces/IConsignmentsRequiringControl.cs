namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IConsignmentsRequiringControlPage
    {
        void ClickCHEDReferencNum();
        bool IsPageLoaded();
        bool VerifyControlStatus(string controlStatus);
        bool VerifyNotificationStatus(string chedRef, string status);
        bool VerifyLink(string link);
        bool VerifyDropdownFieldValue(string field, string value);
        void SelectControlStatus(string field, string value);
        bool VerifyTheControlStatus(string status);
        bool VerifyTheControlStatus(string controlRequired, string sealChkRequired);
        void ClickSearchButton();
        void EnterStartDate(string day, string month, string year);
        void EnterEndDate(string day, string month, string year);
        bool VerifyTheResultsInTheDateRange(string startDate, string endDate);
        bool VerifySortByDropdown(string sortBy);
        void ClickLink(string link);
        void ClickFirstNotification();
        void SwitchToPart1Tab();
        void SwitchToPart2Tab();
        bool WaitForStatusWithSearch(string expectedStatus);
    }
}
