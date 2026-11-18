namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IIdmWhatAreTheBusinessContactDetailsPage
    {
        bool IsPageLoaded();
        void EnterContactDetailsAndContinue(string phone, string email);
    }
}
