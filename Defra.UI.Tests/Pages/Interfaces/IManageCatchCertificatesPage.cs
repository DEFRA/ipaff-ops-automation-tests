namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IManageCatchCertificatesPage
    {
        bool IsPageLoaded(string pageTitle);
        void SelectOption(string option);
        bool VerifyNumberOfCertificates(string numberOfCertificates);
        void ClickAddDetailsLink();
    }
}
