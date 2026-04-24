namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ISelectChecksToRecordPage
    {
        bool IsPageLoaded();
        bool IsCheckStillToDo(string checkType);
        void TickAllCheckboxes();
        void ClickContinue();
    }
}