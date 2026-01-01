using System.Text.Json.Serialization;

namespace CrownATTime.Server.Models
{
    public class MicrosoftNewEmailMessage
    {
        [JsonPropertyName("bccRecipients")]
        public List<Recipient> BccRecipients { get; set; }

        [JsonPropertyName("body")]
        public ItemBody Body { get; set; }

        [JsonPropertyName("bodyPreview")]
        public string BodyPreview { get; set; }

        [JsonPropertyName("categories")]
        public List<string> Categories { get; set; }

        [JsonPropertyName("ccRecipients")]
        public List<Recipient> CcRecipients { get; set; }

        [JsonPropertyName("changeKey")]
        public string ChangeKey { get; set; }

        [JsonPropertyName("conversationId")]
        public string ConversationId { get; set; }

        [JsonPropertyName("conversationIndex")]
        public byte[] ConversationIndex { get; set; }

        [JsonPropertyName("createdDateTime")]
        public DateTimeOffset? CreatedDateTime { get; set; }

        [JsonPropertyName("flag")]
        public FollowupFlag Flag { get; set; }

        [JsonPropertyName("from")]
        public Recipient From { get; set; }

        [JsonPropertyName("hasAttachments")]
        public bool? HasAttachments { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("importance")]
        public string Importance { get; set; }

        [JsonPropertyName("inferenceClassification")]
        public string InferenceClassification { get; set; }

        [JsonPropertyName("internetMessageHeaders")]
        public List<InternetMessageHeader> InternetMessageHeaders { get; set; }

        [JsonPropertyName("internetMessageId")]
        public string InternetMessageId { get; set; }

        [JsonPropertyName("isDeliveryReceiptRequested")]
        public bool? IsDeliveryReceiptRequested { get; set; }

        [JsonPropertyName("isDraft")]
        public bool? IsDraft { get; set; }

        [JsonPropertyName("isRead")]
        public bool? IsRead { get; set; }

        [JsonPropertyName("isReadReceiptRequested")]
        public bool? IsReadReceiptRequested { get; set; }

        [JsonPropertyName("lastModifiedDateTime")]
        public DateTimeOffset? LastModifiedDateTime { get; set; }

        [JsonPropertyName("parentFolderId")]
        public string ParentFolderId { get; set; }

        [JsonPropertyName("receivedDateTime")]
        public DateTimeOffset? ReceivedDateTime { get; set; }

        [JsonPropertyName("replyTo")]
        public List<Recipient> ReplyTo { get; set; }

        [JsonPropertyName("sender")]
        public Recipient Sender { get; set; }

        [JsonPropertyName("sentDateTime")]
        public DateTimeOffset? SentDateTime { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonPropertyName("toRecipients")]
        public List<Recipient> ToRecipients { get; set; }

        [JsonPropertyName("uniqueBody")]
        public ItemBody UniqueBody { get; set; }

        [JsonPropertyName("webLink")]
        public string WebLink { get; set; }

        [JsonPropertyName("attachments")]
        public List<MessageAttachment> Attachments { get; set; }

        [JsonPropertyName("extensions")]
        public List<Extension> Extensions { get; set; }

        [JsonPropertyName("multiValueExtendedProperties")]
        public List<MultiValueLegacyExtendedProperty> MultiValueExtendedProperties { get; set; }

        [JsonPropertyName("singleValueExtendedProperties")]
        public List<SingleValueLegacyExtendedProperty> SingleValueExtendedProperties { get; set; }
    }
    public class Recipient
    {
        [JsonPropertyName("emailAddress")]
        public EmailAddress EmailAddress { get; set; }
    }

    public class EmailAddress
    {
        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class ItemBody
    {
        [JsonPropertyName("contentType")]
        public string ContentType { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }

    public class FollowupFlag
    {
        [JsonPropertyName("flagStatus")]
        public string FlagStatus { get; set; }

        [JsonPropertyName("startDateTime")]
        public DateTimeOffset? StartDateTime { get; set; }

        [JsonPropertyName("dueDateTime")]
        public DateTimeOffset? DueDateTime { get; set; }

        [JsonPropertyName("completionDateTime")]
        public DateTimeOffset? CompletionDateTime { get; set; }
    }

    public class InternetMessageHeader
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class MessageAttachment
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("contentType")]
        public string ContentType { get; set; }

        [JsonPropertyName("size")]
        public long? Size { get; set; }

        [JsonPropertyName("isInline")]
        public bool? IsInline { get; set; }

        [JsonPropertyName("@odata.type")]
        public string ODataType { get; set; }
    }

    public class Extension
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("@odata.type")]
        public string ODataType { get; set; }
    }

    public class MultiValueLegacyExtendedProperty
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("value")]
        public List<string> Value { get; set; }
    }

    public class SingleValueLegacyExtendedProperty
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
