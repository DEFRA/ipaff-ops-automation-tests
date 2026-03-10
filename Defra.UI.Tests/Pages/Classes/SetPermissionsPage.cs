using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll.BoDi;
using SeleniumExtras.WaitHelpers;

namespace Defra.UI.Tests.Pages.Classes
{
    public class SetPermissionsPage : ISetPermissionsPage
    {
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageHeading => _driver.WaitForElement(By.XPath("//h1[contains(@class,'govuk-heading-xl')]"), true);
        private IReadOnlyCollection<IWebElement> collapsePaneHeadings => _driver.FindElements(By.CssSelector("div.govuk-collapse-pane--heading"));
        private IWebElement GetCollapsePaneBody(IWebElement heading) => heading.FindElement(By.XPath("./following-sibling::div[contains(@class,'govuk-collapse-pane--body')]"));
        private IWebElement GetToggleLabelInRow(IWebElement row) => row.FindElement(By.CssSelector("label.govuk-toggle--label"));
        private IWebElement GetToggleInputInRow(IWebElement row) => row.FindElement(By.CssSelector("input.govuk-toggle[type='checkbox']"));
        private IWebElement GetPermissionOnTagInRow(IWebElement row) => row.FindElement(By.CssSelector("span.permission-tag-on"));
        private IReadOnlyCollection<IWebElement> GetToggleRowsInPane(IWebElement pane) => pane.FindElements(By.CssSelector("tbody tr.govuk-table__row"));
        private void WaitForPaneRowsVisible(IWebElement pane) => _driver.WaitForElementCondition(_ => pane.FindElements(By.CssSelector("tbody tr.govuk-table__row")).Any(r => r.Displayed));
        private IWebElement btnFinish => _driver.FindElement(By.XPath("//a[contains(@class,'govuk-button') and normalize-space(text())='Finish']"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public SetPermissionsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return pageHeading.Text.Trim().Contains("Set permissions for");
        }

        public void ToggleAllPermissionsToYes()
        {
            foreach (var heading in collapsePaneHeadings)
            {
                heading.Click();

                var pane = GetCollapsePaneBody(heading);

                WaitForPaneRowsVisible(pane);

                foreach (var row in GetToggleRowsInPane(pane))
                {
                    var permissionOnTag = GetPermissionOnTagInRow(row);
                    var isAlreadyOn = permissionOnTag.IsElementDisplayed();

                    if (!isAlreadyOn)
                    {
                        var toggleLabel = GetToggleLabelInRow(row);
                        toggleLabel.Click();

                        _driver.WaitForElementCondition(
                            d => permissionOnTag.Displayed && !permissionOnTag.GetAttribute("style").Contains("display:none"));
                    }
                }
            }
        }

        public void ClickFinish()
        {
            btnFinish.Click();
        }
    }
}