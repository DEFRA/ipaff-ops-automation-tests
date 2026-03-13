namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IYourIOCHasBeenPutInPlacePage
    {
        bool IsPageLoaded();
        string GetIntensifiedOfficialControlNumber();
        bool IsIntensifiedOfficialControlNumberInCorrectFormat();
    }
}