namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IRecordLaboratoryTestInformationPage
    {
        void EnterSampleUseByDate(string day, string month, string year);
        void EnterReleasedDate(string day, string month, string year);
        bool IsPageLoaded();
        void SelectConclusion(string decision);
        bool IsUseByDatePickerIconDisplayed();
        bool IsReleasedDatePickerIconDisplayed();
        void SelectSampleUseByDateFromDatePicker();
        void SelectReleasedDateFromDatePicker();
        void EnterLabTestMethod(string testMethod);
        void EnterResults(string results);
    }
}