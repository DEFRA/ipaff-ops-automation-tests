namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IIdmCheckYourAnswersPage
    {
        bool IsPageLoaded();
        string GetSummaryValue(string section, string field);
        bool HasChangeLink(string section, string field);
        bool IsRecaptchaAbsent();
        void CompleteRegistration();
    }
}
