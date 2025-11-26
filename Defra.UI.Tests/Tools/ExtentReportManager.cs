using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System.Reflection;

namespace Defra.UI.Tests.Tools
{
    public class ExtentReportManager
    {
        private static ExtentReports _extent;
        private static ExtentSparkReporter _htmlReporter;

        public static ExtentReports GetInstance()
        {
            if (_extent == null)
            {
                var reportDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Reports");

                if (!Directory.Exists(reportDir))
                {
                    Directory.CreateDirectory(reportDir);
                }

                _htmlReporter = new ExtentSparkReporter(Path.Combine(reportDir, $"TestExecutionReport_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.html"));
                _extent = new ExtentReports();
                _extent.AttachReporter(_htmlReporter);
            }
            return _extent;
        }
    }
}