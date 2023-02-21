using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CoreTweet;
using System.Text.Json;

namespace TweetByMisskey
{
    public class Program
    {
        private string WebhookSecret { get; }
        private Tokens Tokens { get; }

        public Program()
        {
            WebhookSecret = Environment.GetEnvironmentVariable("MisskeyWebhookSecret");
            Tokens = Tokens.Create(
                Environment.GetEnvironmentVariable("TwitterConsumerKey"),
                Environment.GetEnvironmentVariable("TwitterConsumerKeySecret"),
                Environment.GetEnvironmentVariable("TwitterAccessToken"),
                Environment.GetEnvironmentVariable("TwitterAccessTokenSecret")
            );
        }

        [FunctionName(nameof(Webhook))]
        public async Task<IActionResult> Webhook(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var misskeyHookSecret = req.Headers["X-Misskey-Hook-Secret"];
            if (misskeyHookSecret != WebhookSecret) return new UnauthorizedResult();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation(requestBody);

            var webhookEvent = JsonSerializer.Deserialize<WebhookPayload>(requestBody);
            if (webhookEvent.EventType != "note") return new OkResult();

            var noteObject = JsonSerializer.Deserialize<WebhookPayloadNoteObject>(requestBody).Body.Note;
            if (!string.IsNullOrEmpty(noteObject.RenoteId) || !string.IsNullOrEmpty(noteObject.ReplyId)) return new OkResult();

            Tweet(noteObject.Text);
            return new OkResult();
        }

        private void Tweet(string text)
        {
            Tokens.Statuses.Update($"{text}");
        }
    }
}
