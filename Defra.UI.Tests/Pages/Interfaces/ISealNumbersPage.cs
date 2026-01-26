namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISealNumbersPage
    {
        bool IsPageLoaded();
        void SelectSealNumRadio(string sealNumOption);
        bool IsSealNumbersNoPreselected();
        void EnterNewSealNumber(string sealNumber);
    }
}