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
    }
}