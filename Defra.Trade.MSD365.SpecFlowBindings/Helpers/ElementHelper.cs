namespace Defra.Trade.Plants.SpecFlowBindings.Helpers;

using OpenQA.Selenium;

public class ElementHelper
{
    public static bool IsElementPresent(IWebDriver driver, By elem)
    {
        try
        {
            driver.FindElement(elem);
            return true;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }
}
