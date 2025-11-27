using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IAdditionalDetailsPage
    {
        bool IsPageLoaded();
        void ClickImportingProduct(string option);
    }
}
