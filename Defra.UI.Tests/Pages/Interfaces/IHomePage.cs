namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IHomePage
    {
        bool IsPageLoaded();
        void ClickApplyForPetTravelDocument();
        void ClickFeedbackLink();
        bool IsNextPageLoaded(string pageTitle);
        void ClickAccessibilityStatementLink();
        void ClickCookiesLink();
        void ClickPrivacyNoticeLink();
        void ClickTermsAndConditionsLink();
        void ClickCrownCopyrightLink();
        bool VerifyTheExpectedStatus(string petName, string status);
        bool VerifyTheApplicationIsNotAvailable(string PetName);
        void ClickViewLink(string petName);
        void ClickOnManageAccountLink();
        void ClickSignOutLink();
        bool VerifyTheLinkOpensInSameTab();
        bool VerifyInvalidDocumentsLink();
        void ClickInvalidDocumentsLink();
        bool InvalidDocsTableHeadings(string petName, string status);
        bool InvalidDocsTablePTDStatus();
        bool InvalidDocsTableViewLink();
        void CloseCurrentTabAndSwitchBack();
        bool IsInvalidDocumentsPageLoaded(string pageTitle);
        bool VerifyManageAccAndSignOutNotVisible();
        bool VerifySuspensionWarning();
        bool VerifyApplyButtonNotVisible();
        bool VerifySuspensionStatusInDashboard(string susStatus);
        bool VerifyCookiesBanner();
        bool VerifyCookiesBannerButtons();
        void ClickAcceptAdditionalCookies();
        bool VerifyAcceptedCookiesConfirmation();
        void ClickRejectAdditionalCookies();
        bool VerifyRejectedCookiesConfirmation();
        void ClickHideCookiesButton(string option);
        bool VerifyCookiesRadioButtons();
        bool VerifyCookiesDefaultSelection();
        void ClickCookiesYesRadioButton();
        void ClickSaveCookiesSettings();
        bool VerifyCookiesSuccessMessage();
        void ClickChangeYourCookieSettings(string option);
        bool VerifyCookiesBannerNotDisplayed();
        bool VerifyCommonHeaderLinks(string govukLink, string takingAPetLink);
    }
}