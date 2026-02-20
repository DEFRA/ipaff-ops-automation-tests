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
        private By successBannerBy => By.Id("upload-success-banner");
        private By successBannerContentBy => By.XPath("//div[@id='upload-success-banner']//p[@class='govuk-body']");
        private By missingDocumentsBannerBy => By.Id("missing-documents-info-banner");
        private By missingDocumentsTitleBy => By.Id("missing-documents-title");
        private By missingDocumentsListBy => By.Id("missing-documents-list");
        private By catchCertificatesUploadedHeadingBy => By.XPath("//h2[contains(text(), 'Catch certificates uploaded')]");
        private By attachmentBlocksBy => By.CssSelector("div.attachment-block");
        private By attachmentCaptionsBy => By.XPath("//span[contains(@class, 'govuk-caption-m')]");
        private By certificateInputBoxesBy => By.XPath("//input[contains(@id, 'number-of-catch-certificates-')]");
        private By updateButtonsBy => By.XPath("//button[contains(@id, 'update-catch-certificates-')]");
        private By addDetailsLinksBy => By.XPath("//a[contains(@id, 'add-') and contains(text(), 'Add details')]");
        private By removeLinksBy => By.XPath("//a[contains(@id, 'remove-') and contains(text(), 'Remove')]");
        private By numberOfCertificatesLabelsBy => By.XPath("//label[contains(text(), 'Number of catch certificates in this attachment')]");
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
            var successBanner = _driver.FindElement(successBannerBy);
            if (!successBanner.Displayed) return false;

            var successContent = _driver.FindElement(successBannerContentBy);
            var text = successContent.Text.Trim();

            return text.Contains($"{expectedFileCount} documents uploaded");
        }

        public bool VerifyNumberOfAttachmentsMissingDetails(int expectedMissingCount)
        {
            var missingBanner = _driver.FindElement(missingDocumentsBannerBy);
            if (!missingBanner.Displayed) return false;

            var missingTitle = _driver.FindElement(missingDocumentsTitleBy);
            var titleText = missingTitle.Text.Trim();

            var missingList = _driver.FindElement(missingDocumentsListBy);
            var listItems = missingList.FindElements(By.TagName("li"));

            return titleText.Contains($"{expectedMissingCount} attachments are missing details") &&
                   listItems.Count == expectedMissingCount;
        }

        public bool VerifyCatchCertificatesUploadedHeadingDisplayed()
        {
            var heading = _driver.FindElement(catchCertificatesUploadedHeadingBy);
            return heading.Displayed &&
                   heading.Text.Trim().Equals("Catch certificates uploaded", StringComparison.OrdinalIgnoreCase);
        }

        public bool VerifyNumberOfCatchCertificatesDisplayedForEachAttachment(int expectedAttachmentCount)
        {
            var labels = _driver.FindElements(numberOfCertificatesLabelsBy);
            var attachmentBlocks = _driver.FindElements(attachmentBlocksBy);

            return labels.Count == expectedAttachmentCount &&
                   attachmentBlocks.Count == expectedAttachmentCount &&
                   labels.All(label => label.Displayed &&
                              label.Text.Contains("Number of catch certificates in this attachment"));
        }

        public bool VerifyEachAttachmentDisplayedAsXofYFormat(int totalAttachments)
        {
            var captions = _driver.FindElements(attachmentCaptionsBy);

            if (captions.Count != totalAttachments)
            {
                return false;
            }

            for (int i = 0; i < totalAttachments; i++)
            {
                var expectedText = $"Attachment {i + 1} of {totalAttachments}";
                if (!captions[i].Text.Trim().Equals(expectedText, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        public bool VerifyEachAttachmentHasPopulatedInputBox(int expectedAttachmentCount)
        {
            var inputBoxes = _driver.FindElements(certificateInputBoxesBy);

            return inputBoxes.Count == expectedAttachmentCount &&
                   inputBoxes.All(input =>
                       input.Displayed &&
                       !string.IsNullOrWhiteSpace(input.GetAttribute("value")));
        }

        public bool VerifyEachAttachmentHasUpdateButton(int expectedAttachmentCount)
        {
            var updateButtons = _driver.FindElements(updateButtonsBy);

            return updateButtons.Count == expectedAttachmentCount &&
                   updateButtons.All(btn => btn.Displayed &&
                                            btn.Text.Trim().Equals("Update", StringComparison.OrdinalIgnoreCase));
        }

        public bool VerifyEachAttachmentHasAddDetailsAndRemoveLinks(int expectedAttachmentCount)
        {
            var addDetailsLinks = _driver.FindElements(addDetailsLinksBy);
            var removeLinks = _driver.FindElements(removeLinksBy);

            var addDetailsValid = addDetailsLinks.Count == expectedAttachmentCount &&
                                 addDetailsLinks.All(link => link.Displayed &&
                                                            link.Text.Contains("Add details"));

            var removeLinksValid = removeLinks.Count == expectedAttachmentCount &&
                                  removeLinks.All(link => link.Displayed &&
                                                         link.Text.Contains("Remove"));

            return addDetailsValid && removeLinksValid;
        }

        public void ClickViewOrAmendDetailsLinkForAttachment(int attachmentNumber)
        {
            var attachmentBlocks = _driver.FindElements(attachmentBlocksBy);

            if (attachmentNumber < 1 || attachmentNumber > attachmentBlocks.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(attachmentNumber),
                    $"Attachment number {attachmentNumber} is out of range. Available attachments: {attachmentBlocks.Count}");
            }

            var targetBlock = attachmentBlocks[attachmentNumber - 1];
            var viewAmendLink = targetBlock.FindElement(By.XPath(".//a[contains(text(), 'View or amend details')]"));
            viewAmendLink.Click();
        }
    }
}