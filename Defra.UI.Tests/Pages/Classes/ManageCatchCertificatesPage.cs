using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class ManageCatchCertificatesPage : IManageCatchCertificatesPage
    {
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement primaryTitle => _driver.WaitForElement(By.Id("page-primary-title"), true);
        private IWebElement rdoMoreCertificateOption(string option) => _driver.FindElement(By.XPath($"//input[@type='radio' and @value='{option}']"));
        private IReadOnlyCollection<IWebElement> RemoveCertificates => _driver.WaitForElements(By.XPath("//a[normalize-space()='Remove']"));
        private IWebElement lnkAddDetails => _driver.FindElement(By.LinkText("Add details"));
        private IWebElement successBanner => _driver.FindElement(By.Id("upload-success-banner"));
        private IWebElement successBannerContent => _driver.FindElement(By.XPath("//div[@id='upload-success-banner']//p[@class='govuk-body']"));
        private IWebElement missingDocumentsBanner => _driver.FindElement(By.Id("missing-documents-info-banner"));
        private IWebElement missingDocumentsTitle => _driver.FindElement(By.Id("missing-documents-title"));
        private IWebElement missingDocumentsList => _driver.FindElement(By.Id("missing-documents-list"));
        private IWebElement catchCertificatesUploadedHeading => _driver.FindElement(By.XPath("//h2[contains(text(), 'Catch certificates uploaded')]"));
        private IReadOnlyCollection<IWebElement> attachmentBlocks => _driver.FindElements(By.CssSelector("div.attachment-block"));
        private IReadOnlyCollection<IWebElement> attachmentCaptions => _driver.FindElements(By.XPath("//span[contains(@class, 'govuk-caption-m')]"));
        private IReadOnlyCollection<IWebElement> certificateInputBoxes => _driver.FindElements(By.XPath("//input[contains(@id, 'number-of-catch-certificates-')]"));
        private IReadOnlyCollection<IWebElement> updateButtons => _driver.FindElements(By.XPath("//button[contains(@id, 'update-catch-certificates-')]"));
        private IReadOnlyCollection<IWebElement> addDetailsLinks => _driver.FindElements(By.XPath("//a[contains(@id, 'add-') and contains(text(), 'Add details')]"));
        private IReadOnlyCollection<IWebElement> removeLinks => _driver.FindElements(By.XPath("//a[contains(@id, 'remove-') and contains(text(), 'Remove')]"));
        private IReadOnlyCollection<IWebElement> numberOfCertificatesLabels => _driver.FindElements(By.XPath("//label[contains(text(), 'Number of catch certificates in this attachment')]"));
        private By missingDocumentsListItemsBy => By.TagName("li");
        private By viewAmendDetailsLinkBy => By.XPath(".//a[contains(text(), 'View or amend details')]");
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public ManageCatchCertificatesPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded(string pageTitle)
        {
            return primaryTitle.Text.Trim().Equals(pageTitle, StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifyNumberOfCertificates(string numberOfCertificates) => RemoveCertificates.Count.ToString() == numberOfCertificates;

        public void SelectOption(string option)
        {
            rdoMoreCertificateOption(option).Click();
        }

        public void ClickAddDetailsLink()
        {
            lnkAddDetails.Click();
        }

        public bool VerifySuccessMessageDisplaysNumberOfFilesAdded(int expectedFileCount)
        {
            return successBanner.Displayed &&
                   successBannerContent.Text.Trim().Contains($"{expectedFileCount} documents uploaded");
        }

        public bool VerifyNumberOfAttachmentsMissingDetails(int expectedMissingCount)
        {
            var listItems = missingDocumentsList.FindElements(missingDocumentsListItemsBy);

            return missingDocumentsBanner.Displayed &&
                   missingDocumentsTitle.Text.Trim().Contains($"{expectedMissingCount} attachments are missing details") &&
                   listItems.Count == expectedMissingCount;
        }

        public bool VerifyCatchCertificatesUploadedHeadingDisplayed()
        {
            return catchCertificatesUploadedHeading.Displayed &&
                   catchCertificatesUploadedHeading.Text.Trim().Equals("Catch certificates uploaded", StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifyNumberOfCatchCertificatesDisplayedForEachAttachment(int expectedAttachmentCount)
        {
            return numberOfCertificatesLabels.Count == expectedAttachmentCount &&
                   attachmentBlocks.Count == expectedAttachmentCount &&
                   numberOfCertificatesLabels.All(label => label.Displayed &&
                       label.Text.Contains("Number of catch certificates in this attachment"));
        }

        public bool VerifyEachAttachmentDisplayedAsXofYFormat(int totalAttachments)
        {
            return attachmentCaptions.Count == totalAttachments &&
                   attachmentCaptions
                       .Select((caption, i) => new { caption, i })
                       .All(x => x.caption.Text.Trim().Equals($"Attachment {x.i + 1} of {totalAttachments}", StringComparison.OrdinalIgnoreCase));
        }

        public bool VerifyEachAttachmentHasPopulatedInputBox(int expectedAttachmentCount)
        {
            return certificateInputBoxes.Count == expectedAttachmentCount &&
                   certificateInputBoxes.All(input =>
                       input.Displayed &&
                       !string.IsNullOrWhiteSpace(input.GetAttribute("value")));
        }

        public bool VerifyEachAttachmentHasUpdateButton(int expectedAttachmentCount)
        {
            return updateButtons.Count == expectedAttachmentCount &&
                   updateButtons.All(btn => btn.Displayed &&
                                            btn.Text.Trim().Equals("Update", StringComparison.OrdinalIgnoreCase));
        }

        public bool VerifyEachAttachmentHasAddDetailsAndRemoveLinks(int expectedAttachmentCount)
        {
            return addDetailsLinks.Count == expectedAttachmentCount &&
                   addDetailsLinks.All(link => link.Displayed && link.Text.Contains("Add details")) &&
                   removeLinks.Count == expectedAttachmentCount &&
                   removeLinks.All(link => link.Displayed && link.Text.Contains("Remove"));
        }

        public void ClickViewOrAmendDetailsLinkForAttachment(int attachmentNumber)
        {
            if (attachmentNumber < 1 || attachmentNumber > attachmentBlocks.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(attachmentNumber),
                    $"Attachment number {attachmentNumber} is out of range. Available attachments: {attachmentBlocks.Count}");
            }

            var targetBlock = attachmentBlocks.ElementAt(attachmentNumber - 1);
            targetBlock.FindElement(viewAmendDetailsLinkBy).Click();
        }
    }
}