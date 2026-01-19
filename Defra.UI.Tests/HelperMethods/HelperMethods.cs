using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using Defra.UI.Tests.Tools;

namespace Defra.UI.Tests.HelperMethods
{
    internal static class HelperMethods
    {
        public static void SelectFromDropdown(this IWebDriver driver, IWebElement Element, string text)
        {
            SelectElement dropDown = new SelectElement(Element);
            dropDown.SelectByText(text);
        }

        private static void SelectFromDropdownJS(this IWebDriver driver, IWebElement Element, string value)
        {
            string script = $"const element = arguments[0]; element.value = '" + value + "'; element.dispatchEvent(new Event('change')); element.dispatchEvent(new Event('blur'));";
            ((IJavaScriptExecutor)driver).ExecuteScript($"{script}", Element);
        }

        public static void ClickRadioButton(this IWebDriver driver, string code)
        {
            IWebElement commLabel = driver.WaitForElement(By.XPath($"//label[contains(.,'{code}')]"));
            bool commRadioButton = commLabel.Text.Contains(code);
            if (commRadioButton)
            {
                var eleme = driver.FindElements(By.TagName("input"));
                eleme.LastOrDefault().Click();
            }
        }

        public static IReadOnlyCollection<IWebElement> GetRadioButtonChildElements(this IWebDriver driver, string code)
        {
            IReadOnlyCollection<IWebElement> commLabel = driver.WaitForElements(By.XPath($"//label[contains(.,'{code}')]"));
            return commLabel;
        }

        public static void ClickFirstRadioButton(this IWebDriver driver, string code)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.XPath($"//label[contains(text(),'{code}')]")).Text.Contains(code));
        }

        public static void ClickRadioButtonOption(this IWebDriver driver, string radioOption)
        {
            IWebElement radioLabel = driver.WaitForElement(By.XPath($"//label[contains(.,'{radioOption}')]"));
            radioLabel.Click();
        }

        public static void ClearWebField(this IWebDriver driver, IWebElement Element)
        {
            while (!Element.GetAttribute("value").Equals("") | !Element.Text.Contains(""))
            {
                Element.SendKeys(Keys.Backspace);
            }
        }

        public static void ContinueButton(this IWebDriver driver)
        {
            IWebElement? continueLabel = null;
            try
            {
                continueLabel = driver.FindElement(By.XPath("//button[normalize-space(text())='Continue']"));
            }
            catch
            {
                continueLabel = driver.WaitForElement(By.XPath("//button[normalize-space(text())='Continue']"), true);
            }

            continueLabel.Click();
        }

        public static void ParhauButton(this IWebDriver driver)
        {
            IWebElement? continueLabel = null;
            try
            {
                continueLabel = driver.FindElement(By.XPath("//button[normalize-space(text())='Parhau']"));
            }
            catch
            {
                continueLabel = driver.WaitForElement(By.XPath("//button[normalize-space(text())='Parhau']"), true);
            }

            continueLabel.Click();
        }

        public static void ClickBrowserBackButton(this IWebDriver driver)
        {
            driver.Navigate().Back();
        }

        public static void ClickBrowserForwardButton(this IWebDriver driver)
        {
            driver.Navigate().Forward();
        }
    }
}