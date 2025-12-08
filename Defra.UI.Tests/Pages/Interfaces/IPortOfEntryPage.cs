using DocumentFormat.OpenXml.Drawing.Charts;
using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IPortOfEntryPage
    {
        bool IsPageLoaded();
        bool IsBCPOrPortOfEntryPageLoaded();
        void EnterPortOfEntry(string port);
        void SelectMeansOfTransport(string mode);
        void EnterTransportId(string transId);
        void EnterTransportDocRef(string DocumentRef);
        void EnterEstimatedArrivalDate(string day, string month, string year);
        void EnterEstimatedArrivalTime(string hour, string minutes);
        void EnterEstimatedJourneyTime(string hours);
    }
}
