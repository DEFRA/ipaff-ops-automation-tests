namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICloneAHealthOrPhytosanitaryCertificate
    {
        bool IsPageLoaded();
        bool VerifyClonePageDisplayText();
        void SelectImportingOption(string importOption);
        void ContinueButton();
    }
}
