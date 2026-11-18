namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IIdmCreateMemorableWordPage
    {
        bool IsPageLoaded();
        void EnterWordAndHintAndContinue(string word, string hint);
    }
}
