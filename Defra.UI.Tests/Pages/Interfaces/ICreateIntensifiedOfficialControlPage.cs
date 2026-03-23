namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICreateIntensifiedOfficialControlPage
    {
        bool IsPageLoaded();
        void EnterCertificateNumber(string certificateNumber);
        void ClickSearchForEstablishment();
        bool IsEstablishmentPopulated(string name, string country, string approvalNumber);
        void EnterCommodityCode(string commodityCode);
        void ClickSearchCommodities();
        bool IsCommodityPopulated(string code, string description);
        void ClickSearchForHazard();
        bool IsHazardPopulated(string hazardName);
        void EnterNetWeight(string netWeight);
        void ClickPlaceUnderIntensifiedOfficialControls();
    }
}