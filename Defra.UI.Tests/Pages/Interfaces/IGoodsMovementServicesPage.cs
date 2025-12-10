namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IGoodsMovementServicesPage
    {
        bool IsPageLoaded();
        void CTCToMoveGoods(string option);
        void GVMSToMoveGoods(string option);
    }
}