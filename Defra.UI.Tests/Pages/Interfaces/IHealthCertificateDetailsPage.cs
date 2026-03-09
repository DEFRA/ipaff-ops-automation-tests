using Defra.UI.Tests.Contracts;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IHealthCertificateDetailsPage
    {
        bool IsPageLoaded();
        Dictionary<string, string> GetKeyAndValuesOfSummaryAndGoods();
    }
}
