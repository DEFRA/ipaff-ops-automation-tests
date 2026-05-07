using System.Collections.Generic;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICsvFileDetailsAndStatusPage
    {
        bool IsPageLoaded();
        IDictionary<string, string> GetSummaryDetails();
        int GetSummaryFieldAsInt(string field);
        void ClickPhsiReportingLink();
    }
}