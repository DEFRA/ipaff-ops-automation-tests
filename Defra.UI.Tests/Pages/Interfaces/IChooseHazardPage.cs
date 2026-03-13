namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IChooseHazardPage
    {
        bool IsPageLoaded();
        void SelectHazardCategory(string category);
        void SelectHazardSubcategory(string subcategory);
        void ClickSearch();
        bool AreAllResultsForSubcategory(string subcategory);
        void SelectHazardByLabTestName(string labTestName);
    }
}