using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using Defra.UI.Framework.Object;
using mailinator_csharp_client;
using mailinator_csharp_client.Models.Messages.Entities;
using mailinator_csharp_client.Models.Messages.Requests;
using Reqnroll;

namespace Defra.UI.Tests.HelperMethods
{
    public interface IFetchCodeFromEmail
    {
        string DomainName { get; }
        Task<string> GetCodeFromEmail(string inboxIdToReadCode);
    }

    public class FetchCodeFromEmail : IFetchCodeFromEmail
    {
        private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(90);
        private static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(3);

        private readonly MailinatorClient _mailinatorClient = new("33805b1795e14f1c9af8272c25684f49");

        public string DomainName { get; } = "team707045.testinator.com";

        public FetchCodeFromEmail(ScenarioContext scenarioContext)
        {
        }

        public async Task<string> GetCodeFromEmail(string inboxIdToReadCode)
        {
            var stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed < Timeout)
            {
                try
                {
                    var inbox = await _mailinatorClient.MessagesClient.FetchInboxAsync(new FetchInboxRequest
                    {
                        Domain = DomainName,
                        Inbox = inboxIdToReadCode,
                        Skip = 0,
                        Limit = 10,
                        Sort = Sort.desc
                    });

                    foreach (var message in inbox?.Messages ?? [])
                    {
                        var response = await _mailinatorClient.MessagesClient.FetchMessageAsync(new FetchMessageRequest
                        {
                            Domain = DomainName,
                            Inbox = inboxIdToReadCode,
                            MessageId = message.Id
                        });

                        foreach (var part in response?.Parts ?? [])
                        {
                            var code = ExtractCode(part.Body);
                            if (!string.IsNullOrEmpty(code))
                                return code;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogMessage("While trying to read the message from Inbox... " + ex.Message);
                }

                await Task.Delay(PollInterval);
            }

            throw new TimeoutException($"No confirmation code was received for {inboxIdToReadCode}@{DomainName} within {Timeout.TotalSeconds} seconds.");
        }

        private static string ExtractCode(string? rawBody)
        {
            if (string.IsNullOrEmpty(rawBody))
                return string.Empty;

            const string startMarker = "Your confirmation code is:";
            const string endMarker = "This code will expire";

            var body = WebUtility.HtmlDecode(Regex.Replace(rawBody, "<[^>]+>", " "));
            var start = body.IndexOf(startMarker, StringComparison.OrdinalIgnoreCase);
            if (start < 0)
                return string.Empty;

            start += startMarker.Length;
            var end = body.IndexOf(endMarker, start, StringComparison.OrdinalIgnoreCase);
            return end > start ? Regex.Replace(body[start..end], @"\s+", string.Empty) : string.Empty;
        }
    }
}
