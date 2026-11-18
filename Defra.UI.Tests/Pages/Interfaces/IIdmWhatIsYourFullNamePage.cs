namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IIdmWhatIsYourFullNamePage
    {
        bool IsPageLoaded();
        void EnterNameAndContinue(string name);
    }
}
