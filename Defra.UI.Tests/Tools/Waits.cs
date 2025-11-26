using Defra.UI.Tests.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Defra.UI.Tests.Tools
{
    internal static class Waits
    {
        private static int GlobalWaits => ConfigSetup.BaseConfiguration.TestConfiguration.GlobalWaitsInSeconds;

        private static readonly string[] browserList = ["SAFARI", "FIREFOX", "CHROMIUM", "SAMSUNG"];

        public static IWebElement WaitForElement(this IWebDriver driver, By elementBy, bool forceWait = false)
        {
            try
            {
                if (browserList.Contains(ConfigSetup.BaseConfiguration.UiFrameworkConfiguration.Target.ToUpper()) && forceWait)
                { Thread.Sleep(TimeSpan.FromSeconds(6)); }

                WebDriverWait driverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(GlobalWaits));
                var element = driverWait.Until(ExpectedConditions.ElementIsVisible(elementBy));

                element.ScrollToElement(driver);

                return element;
            }
            catch (Exception ex)
            {
                var element = driver.FindElement(elementBy);
                element.ScrollToElement(driver);
                return element;

                throw new Exception("Element is not visible", ex);
            }
        }

        public static void Click(this IWebElement elemnt, IWebDriver driver)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView()", elemnt);
            elemnt.Click();
        }

        public static IReadOnlyCollection<IWebElement> WaitForElements(this IWebDriver driver, By elementBy, bool forceWait = false)
        {
            try
            {
                if (browserList.Contains(ConfigSetup.BaseConfiguration.UiFrameworkConfiguration.Target.ToUpper()) && forceWait)
                { Thread.Sleep(TimeSpan.FromSeconds(6)); }

                WebDriverWait driverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(GlobalWaits));
                driverWait.Until(ExpectedConditions.ElementIsVisible(elementBy));
                return driver.FindElements(elementBy);
            }
            catch (Exception ex)
            {
                return driver.FindElements(elementBy);

                throw new Exception("Element is not visible", ex);
            }

        }

        public static TResult WaitForElementCondition<TResult>(this IWebDriver driver, Func<IWebDriver, TResult> condition)
        {
            try
            {
                WebDriverWait driverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(GlobalWaits));
                return driverWait.Until(condition);
            }
            catch (Exception ex)
            {
                throw new Exception("Element exception " + ex.Message);
            }
        }

        public static void WaitForAjax(this IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(d => (bool)(d as IJavaScriptExecutor).ExecuteScript("return document.readyState").ToString().Equals("complete"));
        }

        public static bool WaitForSpinnerToAppearAndDisappear(this IWebDriver driver, By elementBy)
        {
            try
            {
                WebDriverWait driverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                return driverWait.Until(ExpectedConditions.InvisibilityOfElementLocated(elementBy));
            }
            catch (Exception ex)
            {
                throw new Exception("Loading spinner has not disappeared" + ex);
            }
        }

        public static void ElementImplicitWait(this IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
        }

        public static IWebElement WaitForElementClickable(this IWebDriver driver, By elementBy)
        {
            try
            {
                WebDriverWait driverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(GlobalWaits));
                return driverWait.Until(ExpectedConditions.ElementToBeClickable(elementBy));
            }
            catch (Exception ex)
            {
                throw new Exception("Element exception " + ex.Message);
            }
        }

        public static IWebElement WaitForElementExists(this IWebDriver driver, By elementBy, bool forceWait = false)
        {
            try
            {
                if (browserList.Contains(ConfigSetup.BaseConfiguration.UiFrameworkConfiguration.Target.ToUpper()) && forceWait)
                { Thread.Sleep(TimeSpan.FromSeconds(6)); }

                WebDriverWait driverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(GlobalWaits));
                return driverWait.Until(ExpectedConditions.ElementExists(elementBy));
            }
            catch (Exception ex)
            {
                return driver.FindElement(elementBy);

                throw new Exception("Element is not visible", ex);
            }

        }

        public static void Wait(this IWebDriver driver, int seconds)
        {
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
        }
    }
}
