namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IIdmCreatePasswordPage
    {
        bool IsPageLoaded();
        void EnterPasswordAndContinue(string password);
    }
}
