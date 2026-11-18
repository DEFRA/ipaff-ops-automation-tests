namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IIdmWhatIsYourNamePage
    {
        bool IsPageLoaded();
        void EnterNameAndContinue(string firstName, string lastName);
    }
}
