namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISubmitCommodityRulesCsvPage
    {
        bool IsPageLoaded();
        bool WaitForFirstRecordStatus(string expectedStatus, int timeoutSeconds = 60);
        void ClickFirstRecordActionLink();
    }
}