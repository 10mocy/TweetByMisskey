using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TweetByMisskey
{
    public class WebhookPayload
    {
        [JsonPropertyName("hookId")]
        public string Id { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("eventId")]
        public string EventId { get; set; }

        [JsonPropertyName("createdAt")]
        public long CreatedAt { get; set; }

        [JsonPropertyName("type")]
        public string EventType { get; set; }

        [JsonIgnore]
        public string Body { get; set; }
    }

    public class WebhookPayloadNoteObject
    {
        [JsonPropertyName("body")]
        public BodyObject Body { get; set; }
    }

    public class BodyObject
    {
        [JsonPropertyName("note")]
        public NoteObject Note { get; set; }
    }

    public class NoteObject
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("createdAt")]
        public string CreatedAt { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("cw")]
        public string IsNSFW { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("visibility")]
        public string Visibility { get; set; }

        [JsonPropertyName("replyId")]
        public string ReplyId { get; set; }

        [JsonPropertyName("renoteId")]
        public string RenoteId { get; set; }
    }
}
