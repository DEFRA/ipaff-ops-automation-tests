namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IAddAnAgentPage
    {
        bool IsPageLoaded();
        void EnterAgentCode(string agentCode);
        void SelectYesForIsThisTheAgent();
        void TickDelegationCheckbox();
    }
}