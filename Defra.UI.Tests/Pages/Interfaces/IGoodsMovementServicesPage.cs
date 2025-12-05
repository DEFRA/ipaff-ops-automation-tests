namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IGoodsMovementServicesPage
    {
        bool IsPageLoaded();
        void CTCToMoveGoods(string option);
        bool SelectGvmsRadio(string option);
    }
}