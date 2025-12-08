using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface IIdentityAndPhysicalChecksPage
    { 
        bool IsPageLoaded();
        bool IsIdentityPhysicalAndWelfareChecksPageLoaded();
        void ClickIdentityCheckOption(string decision, string checkType);
        void ClickPhysicalCheckDecision(string decision);
        void ClickSaveAndContinue();
        void SelectIdentityCheck(string decision);
        void SelectPhysicalCheck(string decision);
        void SelectWelfareCheck(string decision);
        void EnterNumberOfAnimalsChecked(string numberOfAnimals);
        void EnterNumberOfDeadAnimals(string numberOfDeadAnimals, string unit);
        void EnterNumberOfUnfitAnimals(string numberOfUnfitAnimals, string unit);
        void EnterNumberOfBirthsOrAbortions(string numberOfBirths);
    }
}
