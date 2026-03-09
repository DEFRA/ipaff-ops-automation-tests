using Defra.UI.Tests.Contracts;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICertificateDetailsPage
    {
        bool IsPageLoaded();
        bool VerifyCertificateDetailsPageSubtitles();
        void SelectCountryOfOrigin(string countryOdOrigin);
        void EnterCertificateReferenceNumber(string referencNumber);
        void EnterCertificateDateOfIssueYear(int day, int month, int year);
        void EnterConsignerConsigneeImporterName(string name);
        NotificationDetails GetNotificationDetailsForCloningCHEDPP();
        void ClickSearchButton();
        NotificationDetails GetNotificationDetailsForCloningCHEDA();
    }
}
