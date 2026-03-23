using Defra.UI.Tests.Pages.Interfaces;
using Defra.UI.Tests.Tools;
using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Pages.Classes
{
    public class IOCDetailsPage : IIOCDetailsPage
    {
        private IObjectContainer _objectContainer;

        #region Page Objects
        private IWebElement pageTitle => _driver.WaitForElement(By.XPath("//h1[contains(@class,'govuk-heading-xl')]"), true);
        private IWebElement btnStopControl => _driver.FindElement(By.Id("stop-control-button"));
        private IWebElement GetCheckedConsignmentRowByChedRef(string chedRef) => _driver.FindElement(By.XPath($"//table[@id='checked-consignments-table']//tbody//tr[td[contains(@class,'consignment-reference') and .//a[normalize-space()='{chedRef}']]]"));
        private IWebElement GetAssociatedChedPRowByChedRef(string chedRef) => _driver.FindElement(By.XPath($"//table[@id='associated-chedp-table']//tbody//tr[td[contains(@class,'consignment-reference') and .//a[normalize-space()='{chedRef}']]]"));
        #endregion

        private IWebDriver _driver => _objectContainer.Resolve<IWebDriver>();

        public IOCDetailsPage(IObjectContainer container)
        {
            _objectContainer = container;
        }

        public bool IsPageLoaded()
        {
            return pageTitle.Text.StartsWith("IOC.", StringComparison.OrdinalIgnoreCase);
        }

        public void ClickStopControl()
        {
            btnStopControl.Click();
        }

        public bool IsUnderCheckedConsignments(string chedRef)
        {
            try
            {
                return GetCheckedConsignmentRowByChedRef(chedRef).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool IsUnderCheckedConsignmentsWithCount(string chedRef, string count)
        {
            try
            {
                var row = GetCheckedConsignmentRowByChedRef(chedRef);
                var countCell = row.FindElement(By.XPath(".//td[1]"));
                return countCell.Text.Trim().Equals(count, StringComparison.OrdinalIgnoreCase);
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool IsUnderAssociatedChedP(string chedRef)
        {
            try
            {
                return GetAssociatedChedPRowByChedRef(chedRef).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public string? GetCheckedConsignmentCount(string chedRef)
        {
            var row = GetCheckedConsignmentRowByChedRef(chedRef);
            return row.FindElement(By.XPath(".//td[1]")).Text.Trim();
        }
    }
}