namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IRecordPhsiChecksPage
    {
        bool IsPageLoaded();
        void RecordAllCompliantDecisionsAcrossAllPages(string decision);
    }
}