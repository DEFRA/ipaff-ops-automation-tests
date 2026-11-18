namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IIdmWhatIsYourPhoneNumberPage
    {
        bool IsPageLoaded();
        void EnterPhoneAndContinue(string phone);
    }
}
