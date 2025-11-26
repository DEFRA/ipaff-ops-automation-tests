using Defra.UI.Framework.Configuration;

namespace Defra.UI.Tests.Configuration
{
    public class UiFrameworkConfiguration : IFrameworkConfiguration
    {

        private string _grid;
        public string Target { get; set; }
        public bool IsDebug { get; set; }


        public string SeleniumGrid
        {
            get => _grid ?? string.Empty;
            set => _grid = value;
        }
    }
}
