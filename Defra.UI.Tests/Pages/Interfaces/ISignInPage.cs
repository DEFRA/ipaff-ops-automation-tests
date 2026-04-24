namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISignInPage
    {
        public bool IsPageLoaded();
        public void ClickCreateSignInDetailsLink();
        void SignIn(string userName, string password);
        public void ClickSignedOut();
        public bool IsSignedOut();
        public bool IsSuccessfullySignedOut();
        public void SignInToDynamics(string username, string password);
        public void CPSignIn(string userName, string password);
        void ClickPetsTravelApplicationPortalLink();
        void ClickSignInButton();
        bool IsError(string errorMessage);
        bool IsSignedOutFromYourDefraAccountPage();
        void EnterPassword();
        void IPAFFSInternalInspectorSignIn(string userName, string credential);
        bool IsSignedOutFromIOC();
        void IPAFFSSignInViaDynamics(string userName, string password);
    }
}