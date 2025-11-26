
namespace Defra.UI.Tests.Configuration
{
    public  class BaseConfiguration
    {
        public TestConfiguration TestConfiguration { get; set; }
        public UiFrameworkConfiguration UiFrameworkConfiguration { get; set; }
        public BrowserStackConfiguration BrowserStackConfiguration { get; set; }
        public AzureConnectionConfig AzureConnectionConfig { get; set; }
    }
}