namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IIdmYourGovernmentGatewayUserIdIsPage
    {
        bool IsPageLoaded();
        string GetGatewayId();
        void ClickContinueButton();
    }
}
