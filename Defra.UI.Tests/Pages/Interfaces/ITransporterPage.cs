using Faker;
using OpenQA.Selenium;

namespace Defra.UI.Tests.Pages.Interfaces
{
    public interface ITransporterPage
    { 
        bool IsPageLoaded();
        void ClickAddTransporter();
        bool VerifySelectedTransporter();
        void ClickSaveAndContinue();
        bool VerifySelectedTransporter(string name, string address, string country, string approvalNumber, string type);
        void ClickChangeTransporter();
    }
}
