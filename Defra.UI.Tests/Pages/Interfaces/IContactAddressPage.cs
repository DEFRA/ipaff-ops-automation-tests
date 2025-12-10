namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IContactAddressPage
    {
        bool IsPageLoaded();
        bool IsContactAddressForConsignmentPageLoaded();
        bool IsPageLoadedWithoutSecondaryTitle();
        string GetSelectedContactAddress();
    }
}