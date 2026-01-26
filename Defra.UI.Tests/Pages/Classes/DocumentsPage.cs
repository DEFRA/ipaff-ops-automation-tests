using Defra.UI.Tests.Configuration;
using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class DocumentsPage : IDocumentsPage
    {
        private string Platform => ConfigSetup.BaseConfiguration.TestConfiguration.Platform;
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement btnSaveAndContinue => _driver.FindElement(By.Id("button-save-and-continue"));
        private IWebElement btnAddAnotherDocument => _driver.FindElement(By.Id("button-add-additional-document"));
        private By inspectorDocumentRowsBy => By.XPath("//div[@id='inspector-documents-table']//div[contains(@class, 'additional-document-info')]");
        private IWebElement GetInspectorDocumentType(int index) => _driver.FindElement(By.Id($"additional-document-type-value-{index}"));
        private IWebElement GetInspectorDocumentReference(int index) => _driver.FindElement(By.Id($"additional-document-reference-value-{index}"));
        private IWebElement GetInspectorDocumentDateOfIssue(int index) => _driver.FindElement(By.Id($"additional-document-date-value-{index}"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public DocumentsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return primaryTitle.Text.Contains("Documents");
        }

        public void ClickSaveAndContinue()
        {
            btnSaveAndContinue.Click();
        }

        public bool IsAddAnotherDocumentLinkDisplayed()
        {
            return btnAddAnotherDocument.IsElementDisplayed();
        }

        public bool VerifyNoDocumentsInInspectorSection()
        {
            var inspectorDocuments = _driver.FindElements(inspectorDocumentRowsBy);

            if (inspectorDocuments.Count == 0)
            {
                Console.WriteLine("Verified: No documents found in Inspector section");
                return true;
            }

            Console.WriteLine($"Found {inspectorDocuments.Count} document(s) in Inspector section when expecting 0");
            return false;
        }

        public (string? documentType, string? documentReference, string? dateOfIssue) GetInspectorDocumentDetails(int index = 0)
        {
            var documentType = GetInspectorDocumentType(index).SafelyGetText();
            var documentReference = GetInspectorDocumentReference(index).SafelyGetText();
            var dateOfIssue = GetInspectorDocumentDateOfIssue(index).SafelyGetText();

            return (documentType, documentReference, dateOfIssue);
        }
    }
}