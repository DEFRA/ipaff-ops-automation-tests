using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IDecisionPage
    {
        bool IsPageLoaded();
        void SelectAcceptableFor(string acceptableFor, string subOption);
    }
}