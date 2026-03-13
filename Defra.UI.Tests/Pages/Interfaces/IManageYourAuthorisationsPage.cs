namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IManageYourAuthorisationsPage
    {
        bool IsPageLoaded();
        bool IsBusinessNameDisplayedAsHeader(string businessName);
        bool IsChangeSettingsLinkDisplayed();
        void ClickChangeSettingsLink();
        bool IsBusinessesYouAreAuthorisedToRepresentHeaderDisplayed();
        bool IsAgentCodeDisplayed(string agentCode);
        bool IsAutomaticallyAcceptDelegationToggledYes();
        bool IsCompaniesWithNoPermissionsDisplayed();
        bool IsCompaniesWithPermissionsDisplayed();
        bool AreCompaniesListed(string trader1BusinessName, string trader2BusinessName);
        bool IsAgentsActingOnYourBehalfHeaderDisplayed();
        bool IsNoAgentsAuthorisedMessageDisplayed();
        bool IsAddAnAgentButtonDisplayed();
        void ClickAddAnAgent();
        bool IsAgentListedWithConfirmedDelegation(string businessName);
        void ClickBackLink();
    }
}