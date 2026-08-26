namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterHowDoYouWantToAddCommoditiesToYourConsignmentPage
    {
        bool IsPageLoaded();
        void SelectCommodityAddMethod(string method);
        void ClickContinueButton();
    }
}