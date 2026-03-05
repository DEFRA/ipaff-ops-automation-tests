namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICloneAHealthOrPhytosanitaryCertificatePage
    {
        bool IsPageLoaded();
        bool VerifyClonePageDisplayText();
        void SelectImportingOption(string importOption);
        void ContinueButton();
    }
}
