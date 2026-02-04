using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ITransportToBorderControlPostPage
    {
        void SelectInspectionPremises(string premises);
        bool IsPageLoaded();
        void SelectEntryBCP(string entryBCP);
    }
}
