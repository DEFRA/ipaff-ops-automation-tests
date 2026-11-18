namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IIdmWhatIsYourBusinessAddressPage
    {
        bool IsDisplayed();
        void EnterAddressAndContinue(string buildingNumber, string street, string town, string postcode);
    }
}
