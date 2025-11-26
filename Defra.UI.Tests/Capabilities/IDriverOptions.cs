
using OpenQA.Selenium;

namespace Defra.UI.Tests.Capabilities
{
    public interface IDriverOptions
    {
        DriverOptions GetDriverOptions(Dictionary<string, string> capDictionary = null);
    }
}
