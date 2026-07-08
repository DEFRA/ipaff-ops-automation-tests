using Defra.UI.Tests.Configuration;
using Reqnroll.BoDi;

namespace Defra.UI.Tests.Tools
{
    public interface IUrlBuilder
    {
        public UrlBuilder Default();
        public UrlBuilder BTMSDefault();
        public UrlBuilder InspectorDefault();
        public UrlBuilder ExporterPortalDefault();
        public string BuildApp();
        public UrlBuilder Add(string segment);
    }

    public class UrlBuilder : IUrlBuilder
    {
        private IObjectContainer _objectContainer;
        public UrlBuilder(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
            segments = new List<string>();
        }
        private IList<string> segments;
        private bool hasTrailingSlash;
        private string BaseApplicationUrl = null;
        private string BaseBTMSApplicationUrl = null;
        private string BaseInspectorUrl = null;
        private string BaseExporterPortalUrl = null;

        public UrlBuilder Add(string segment)
        {
            if (segment == null)
                throw new ArgumentNullException("segment");

            var cleanSegment = CleanSegment(segment);
            if (!string.IsNullOrEmpty(cleanSegment))
            {
                segments.Add(cleanSegment);
            }

            hasTrailingSlash = segment.EndsWith("/");

            return this;
        }

        public string BuildApp()
        {
            string path = null;
            if (segments.Count > 0)
            {
                path = string.Join("/", segments);

                if (segments.Count > 0 && hasTrailingSlash)
                {
                    path += "/";
                }
                path = BaseApplicationUrl + "/" + path;
            }
            else
            {
                path = BaseApplicationUrl;
            }
            return path;
        }

        public string BuildBTMSApp()
        {
            string path = null;
            if (segments.Count > 0)
            {
                path = string.Join("/", segments);

                if (segments.Count > 0 && hasTrailingSlash)
                {
                    path += "/";
                }
                path = BaseBTMSApplicationUrl + "/" + path;
            }
            else
            {
                path = BaseBTMSApplicationUrl;
            }
            return path;
        }

        public string BuildInspectorApp()
        {
            string path = null;
            if (segments.Count > 0)
            {
                path = string.Join("/", segments);

                if (segments.Count > 0 && hasTrailingSlash)
                {
                    path += "/";
                }
                path = BaseInspectorUrl + "/" + path;
            }
            else
            {
                path = BaseInspectorUrl;
            }
            return path;
        }

        public string BuildExporterPortalApp()
        {
            string path = null;
            if (segments.Count > 0)
            {
                path = string.Join("/", segments);

                if (segments.Count > 0 && hasTrailingSlash)
                {
                    path += "/";
                }
                path = BaseExporterPortalUrl + "/" + path;
            }
            else
            {
                path = BaseExporterPortalUrl;
            }
            return path;
        }

        public UrlBuilder Default()
        {
            BaseApplicationUrl = ConfigSetup.BaseConfiguration.TestConfiguration.ApplicationUrl;

            return this;
        }

        public UrlBuilder BTMSDefault()
        {
            BaseBTMSApplicationUrl = ConfigSetup.BaseConfiguration.TestConfiguration.BTMSApplicationUrl;

            return this;
        }

        public UrlBuilder InspectorDefault()
        {
            BaseInspectorUrl = ConfigSetup.BaseConfiguration.TestConfiguration.InspectorUrl;

            return this;
        }

        public UrlBuilder ExporterPortalDefault()
        {
            BaseExporterPortalUrl = ConfigSetup.BaseConfiguration.TestConfiguration.ExporterPortalUrl;

            return this;
        }

        private static string CleanSegment(string segment)
        {
            var unescaped = Uri.UnescapeDataString(segment);
            return Uri.EscapeUriString(unescaped).Replace("?", "%3F").Trim().TrimStart('/').TrimEnd('/');
        }
    }
}