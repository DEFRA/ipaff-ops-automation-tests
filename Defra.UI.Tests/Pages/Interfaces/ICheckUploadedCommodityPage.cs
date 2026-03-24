using Reqnroll;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ICheckUploadedCommodityPage
    {
        bool IsUploadSuccessMsgDisplayed(string successMsg);
        bool IsCountOfCommodityMatchesWithInput(int expectedCommodityCount);
        bool VerifyInfoMessage(string msgHeading, string msgContent);
        void ValidateAllCommodityDetails(Table? inputAllCommodityData, ref bool allDataMatches, List<string> mismatches);
        void ClickConfirmAndContinueButton();
        bool IsPageLoaded();
        bool WaitForUploadToCompleteAndVerifySuccessMessage(string successMsg);
    }
}
