namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IGovernmentGatewayTypePage
    {
        bool IsPageLoaded(string pageName);
        void SelectLoginType(string loginType);
        void ClickContinueButton();
        void ClickSignInButton();
    }
}
