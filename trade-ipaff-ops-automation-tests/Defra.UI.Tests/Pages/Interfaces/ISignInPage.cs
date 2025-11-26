namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISignInPage
    {
        public bool IsPageLoaded();
        public bool IsSignedIn(string userName, string password);
        public void ClickCreateSignInDetailsLink();
        public void ClickSignedOut();
        public bool IsSignedOut();
        public bool IsSuccessfullySignedOut();
        public void SignInToDynamics(string username, string password);
        public void CPSignIn(string userName, string password);
        void ClickPetsTravelApplicationPortalLink();
        void ClickSignInButton();
        bool IsError(string errorMessage);
        bool IsSignedOutFromYourDefraAccountPage();
    }
}
