using mailinator_csharp_client.Models.Messages.Entities;
using mailinator_csharp_client.Models.Messages.Requests;
using mailinator_csharp_client.Models.Responses;
using mailinator_csharp_client;
using Reqnroll;
using Defra.UI.Framework.Object;

namespace Defra.UI.Tests.HelperMethods
{
    public interface IFetchCodeFromEmail
    {
        public Task<string> GetCodeFromEmail(string inboxIdToReadCode);
        public Task DeleteMessageFromInbox(string inboxId);
        public Task DeleteAllMessagesFromInbox();
        public string DomainName { get; set; }
    }

    public class FetchCodeFromEmail : IFetchCodeFromEmail
    {
        private ScenarioContext ScenarioContext { get; set; }
        public string DomainName { get; set; } = "team553512.testinator.com";

        private MailinatorClient mailinatorClient = new MailinatorClient("af00c8254afc4c34b3f32ba44a040e73");

        public FetchCodeFromEmail(ScenarioContext _scenarioContext)
        {
            ScenarioContext = _scenarioContext;
        }

        public async Task<string> GetCodeFromEmail(string inboxIdToReadCode)
        {
            string code = "";
            try
            {
                //Fetch Inbox
                Thread.Sleep(5000);
                FetchInboxRequest fetchInboxRequest = new FetchInboxRequest() { Domain = DomainName, Inbox = "*", Skip = 0, Limit = 30, Sort = Sort.asc };
                FetchInboxResponse fetchInboxResponse = await mailinatorClient.MessagesClient.FetchInboxAsync(fetchInboxRequest);
                
                var inBoxMessage = fetchInboxResponse.Messages.SingleOrDefault(t => t.To.Equals(inboxIdToReadCode));

                //Fetch Message
                FetchMessageRequest fetchMessageRequest = new FetchMessageRequest() { Domain = DomainName, Inbox = inBoxMessage?.To, MessageId = inBoxMessage?.Id };
                FetchMessageResponse fetchMessageResponse = await mailinatorClient.MessagesClient.FetchMessageAsync(fetchMessageRequest);

                var message = fetchMessageResponse.Parts[0];

                string body = message.Body;
                int pFrom = body.IndexOf("Your confirmation code is:") + "Your confirmation code is:".Length; ;
                int pTo = body.LastIndexOf("This code will expire in 30 minutes");

                code = body.Substring(pFrom, pTo - pFrom).Replace("\r", "").Replace("\n", "");

            }
            catch (Exception ex)
            {
                Logger.LogMessage("While trying to read the message from Inbox... " + ex.Message);
            }

            return code;
        }

        public async Task DeleteMessageFromInbox(string inboxIdToReadCode)
        {
            try
            {
                var code = inboxIdToReadCode.Substring(0, inboxIdToReadCode.IndexOf('-'));
                
                DeleteMessageRequest deleteMessageRequest = new DeleteMessageRequest()
                {
                    Domain = DomainName,
                    Inbox = "*",
                    MessageId = code
                };

                DeleteMessageResponse deleteMessageResponse = await mailinatorClient.MessagesClient.DeleteMessageAsync(deleteMessageRequest);
            }
            catch (Exception ex)
            {
                Logger.LogMessage("While Deleteing the message from Inbox... " + ex.Message);
            }
        }

        public async Task DeleteAllMessagesFromInbox()
        {
            try
            {
                await mailinatorClient.MessagesClient.DeleteAllDomainMessagesAsync(new DeleteAllDomainMessagesRequest
                {
                    Domain = DomainName
                });
            }
            catch (Exception ex)
            {
                Logger.LogMessage("While Deleteing the message from Inbox... " + ex.Message);
            }
        }
    }
}