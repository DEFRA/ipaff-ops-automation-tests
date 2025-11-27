using OpenQA.Selenium;
using System.Security.Cryptography;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IGoodsMovementServicesPage
    {
        bool IsPageLoaded();
        void CTCToMoveGoods(string option);
    }
}
