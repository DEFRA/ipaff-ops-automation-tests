namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IExporterWhoAreYouSendingThisConsignmentToPage
    {
        bool IsPageLoaded();
        void EnterConsigneeDetails(string name, string addressLine1, string addressLine2, string townOrCity, string county, string postCode);
        void ClickSaveAndContinueButton();
    }
}
