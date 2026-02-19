namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICatchCertificatesPage
    {
        bool IsPageLoaded();
        void SelectAddCatchCertificate(string option);
        bool VerifyQuestionDisplayed(string questionText);
        bool VerifyRadioButtonsDisplayed(string yesText, string noText);
    }
}