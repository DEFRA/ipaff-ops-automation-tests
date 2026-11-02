namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IIdmEnterCodeToConfirmYourEmailAddressPage
    {
        bool IsPageLoaded();
        void EnterCodeAndConfirm(string code);
    }
}
