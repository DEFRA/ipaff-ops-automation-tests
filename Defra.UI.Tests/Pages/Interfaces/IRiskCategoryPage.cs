using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IRiskCategoryPage
    {
        bool IsPageLoaded();
        void ClickRiskCategory(string option);
    }
}
