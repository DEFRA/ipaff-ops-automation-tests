namespace Defra.Trade.Plants.SpecFlowBindings.Helpers;

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using OpenQA.Selenium;

/// <summary>
/// Downloads a PDF using the authenticated session of an existing WebDriver.
///
/// The HTTP request is made from the test host (not the browser), using the cookies held by
/// the driver for the PDF's domain. This works the same locally and when the browser runs in
/// a remote Selenium Grid container, because no file ever needs to be written by the browser
/// or copied back from the grid node.
/// </summary>
public static class PdfDownloadHelper
{
    public static string DownloadPdfUsingSession(IWebDriver driver, string pdfUrl, string fileName, int maxRetries = 3)
    {
        if (driver == null) throw new ArgumentNullException(nameof(driver));
        if (string.IsNullOrWhiteSpace(pdfUrl)) throw new ArgumentException("PDF url must be provided.", nameof(pdfUrl));

        var uri = new Uri(pdfUrl);
        string lastError = null;

        for (var attempt = 1; ; attempt++)
        {
            Console.WriteLine($"[PdfDownload] Attempt {attempt} of {maxRetries}: {pdfUrl}");
            var downloadDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(downloadDirectory);

            try
            {
                // Send cookies as a raw header, exactly as the browser would. System.Net.Cookie /
                // CookieContainer reject valid browser cookies whose values contain ',' or ';'
                // (e.g. the JSON cookie-consent cookie set by the phytosanitary service).
                var cookieHeader = string.Join("; ", driver.Manage().Cookies.AllCookies
                    .Select(c => $"{c.Name}={c.Value}"));

                using var handler = new HttpClientHandler { UseCookies = false, AllowAutoRedirect = true };
                using var client = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(60) };
                client.DefaultRequestHeaders.Accept.ParseAdd("application/pdf");
                if (!string.IsNullOrEmpty(cookieHeader))
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Cookie", cookieHeader);

                var userAgent = GetUserAgent(driver);
                if (!string.IsNullOrWhiteSpace(userAgent))
                    client.DefaultRequestHeaders.UserAgent.TryParseAdd(userAgent);

                var bytes = client.GetByteArrayAsync(uri).GetAwaiter().GetResult();

                if (IsPdf(bytes))
                {
                    var filePath = Path.Combine(downloadDirectory, fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) ? fileName : $"{fileName}.pdf");
                    File.WriteAllBytes(filePath, bytes);
                    Console.WriteLine($"[PdfDownload] PDF downloaded: {filePath}");
                    return filePath;
                }

                lastError = $"Response was not a PDF ({bytes.Length} bytes).";
                Console.WriteLine($"[PdfDownload] Attempt {attempt}: {lastError}");
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
                Console.WriteLine($"[PdfDownload] Attempt {attempt} failed: {ex.Message}");
            }

            TryDeleteDirectory(downloadDirectory);

            if (attempt >= maxRetries)
                throw new InvalidOperationException($"PDF failed to download from '{pdfUrl}' using the browser session after {maxRetries} attempts. Last error: {lastError}");

            Thread.Sleep(attempt * 1000);
        }
    }

    private static bool IsPdf(byte[] bytes) =>
        bytes.Length > 4 && bytes[0] == '%' && bytes[1] == 'P' && bytes[2] == 'D' && bytes[3] == 'F';

    private static string GetUserAgent(IWebDriver driver)
    {
        try
        {
            return ((IJavaScriptExecutor)driver).ExecuteScript("return navigator.userAgent")?.ToString();
        }
        catch
        {
            return null;
        }
    }

    private static void TryDeleteDirectory(string directory)
    {
        try
        {
            if (Directory.Exists(directory) && !Directory.EnumerateFileSystemEntries(directory).Any())
                Directory.Delete(directory);
        }
        catch
        {
            // best effort
        }
    }
}
