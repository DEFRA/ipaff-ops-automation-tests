namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IAddConsigneePage
    {
        bool IsPageLoaded();
        void EnterConsigneeName(string name);
        void EnterConsigneeAddress(string address);
        void EnterConsigneeCityOrTown(string cityOrTown);
        void EnterConsigneePostCode(string postCode);
        void EnterConsigneeTelephone(string telephone);

        void EnterConsigneeCountry(string country);
        void EnterConsigneeEmail(string email);
    }
}
