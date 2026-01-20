namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IContactPage
    {
        bool IsPageLoaded();
        public string GetAphaContactText { get; }
        public void ClickBackLink();
    }
}