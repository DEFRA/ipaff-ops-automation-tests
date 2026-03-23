namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IIOCDetailsPage
    {
        bool IsPageLoaded();
        void ClickStopControl();
        bool IsUnderCheckedConsignments(string chedRef);
        bool IsUnderCheckedConsignmentsWithCount(string chedRef, string count);
        bool IsUnderAssociatedChedP(string chedRef);
        string? GetCheckedConsignmentCount(string chedRef);
    }
}