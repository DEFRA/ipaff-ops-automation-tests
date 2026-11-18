namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IIdmEnterYourEmailAddressPage
    {
        bool IsPageLoaded();
        void EnterEmailAddress(string email);
        void ClickContinueButton();
    }
}
