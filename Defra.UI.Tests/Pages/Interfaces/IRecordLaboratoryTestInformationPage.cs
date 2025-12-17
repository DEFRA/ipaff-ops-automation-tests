namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IRecordLaboratoryTestInformationPage
    {
        void EnterSampleUseByDate(string day, string month, string year);
        void EnterReleasedDate(string day, string month, string year);
        bool IsPageLoaded();
        void SelectConclusion(string decision);
    }
}
