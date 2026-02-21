using DocumentFormat.OpenXml.Drawing.Charts;
using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ITransportAfterPortOfEntryPage
    {
        bool IsPageLoaded();
        void SelectMeansOfTransportAfterBCP(string mode);
        void EnterTransportIdentificationAfterBCP(string transportId);
        void EnterTransportDocumentReferenceAfterBCP(string documentRef);
        void EnterDepartureDateFromBCP(DateTime departureDate);
        void EnterDepartureTimeFromBCP(string time);
        string GetMeansOfTransportAfterBCP { get; }
        string GetTransportIdentificationAfterBCP { get; }
        string GetTransportDocumentReferenceAfterBCP { get; }
        string GetDepartureDateFromBCP { get; }
        string GetDepartureTimeFromBCP { get; }
    }
}