using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IOriginOfImportPage
    {
        bool IsPageLoaded();
        void IsRegionOfOriginCodeNeeded(string option);
        void IsConformToRegulatoryRequirements(string option);
        void IsItAfterBCP(string option);
        void EnterConsignmentRefNum(string refNum);
    }
}
