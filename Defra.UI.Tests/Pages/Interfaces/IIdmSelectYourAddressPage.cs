namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IIdmSelectYourAddressPage
    {
        bool IsPageLoaded();
        bool IsDisplayed();
        bool WaitForPage(int timeoutSeconds);
        string SelectAddressAndContinue(string postcode);
    }
}
