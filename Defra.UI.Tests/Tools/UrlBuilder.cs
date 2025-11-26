using Reqnroll.BoDi;
using Defra.UI.Tests.Configuration;

namespace Defra.UI.Tests.Tools
{
    public interface IUrlBuilder
    {
        public UrlBuilder Default();
        public string BuildApp();
        public string BuildCom();
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
        private string BaseComplianceUrl = null;
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
            }else
            {
                path = BaseApplicationUrl;
            }
            return path;
        }

        public string BuildCom()
        {
            string path = null;
            if (segments.Count > 0)
            {
                path = string.Join("/", segments);

                if (segments.Count > 0 && hasTrailingSlash)
                {
                    path += "/";
                }
                path = BaseComplianceUrl + "/" + path;
            }
            else
            {
                path = BaseComplianceUrl;
            }
            return path;
        }

        public UrlBuilder Default()
        {

            BaseComplianceUrl = ConfigSetup.BaseConfiguration.TestConfiguration.ComplianceUrl;

            BaseApplicationUrl = ConfigSetup.BaseConfiguration.TestConfiguration.ApplicationUrl;

            return this;
        }

        private static string CleanSegment(string segment)
        {
            var unescaped = Uri.UnescapeDataString(segment);
            return Uri.EscapeUriString(unescaped).Replace("?", "%3F").Trim().TrimStart('/').TrimEnd('/');
        }
    }
}