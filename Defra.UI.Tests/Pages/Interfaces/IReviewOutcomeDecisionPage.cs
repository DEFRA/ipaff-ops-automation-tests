using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IReviewOutcomeDecisionPage
    {
        bool IsPageLoaded();
        void ClickSubmitDecision();
        void EnterCurrentDateAndTime(string day, string month, string year, string hours, string minutes);
    }
}
