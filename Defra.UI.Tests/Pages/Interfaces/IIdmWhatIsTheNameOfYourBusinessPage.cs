namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IIdmWhatIsTheNameOfYourBusinessPage
    {
        bool IsPageLoaded();
        void EnterBusinessNameAndContinue(string name);
    }
}
