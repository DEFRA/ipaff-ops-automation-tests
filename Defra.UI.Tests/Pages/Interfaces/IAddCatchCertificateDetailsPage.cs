namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IAddCatchCertificateDetailsPage
    {
        bool IsPageLoaded(string pageTitle);
        bool VerifyContent(string content);
        bool VerifyCalendar();
        void EnterCatchCertificateReference(string reference);
        void EnterDateOfIssue(string day, string month, string year);
        void EnterFlagStateOfCatchingVessel(string FlagState);
        void SelectSpecies(string species);
    }
}
