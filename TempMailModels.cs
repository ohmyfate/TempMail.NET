using System.Text.Json.Serialization;

namespace TempMailNET;

public record Attachment(
    [property: JsonPropertyName("attachment_id")] int AttachmentId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("size")] int Size
);

public record MailDetail(
    [property: JsonPropertyName("attachments")] Attachment[] Attachments,
    [property: JsonPropertyName("date")] string Date,
    [property: JsonPropertyName("from")] string From,
    [property: JsonPropertyName("from_is_local")] bool FromIsLocal,
    [property: JsonPropertyName("from_mail")] string FromMail,
    [property: JsonPropertyName("from_name")] string FromName,
    [property: JsonPropertyName("html")] string Html,
    [property: JsonPropertyName("is_tls")] bool IsTls,
    [property: JsonPropertyName("mail_id")] int MailId,
    [property: JsonPropertyName("message_id")] string MessageId,
    [property: JsonPropertyName("result")] bool Result,
    [property: JsonPropertyName("subject")] string Subject,
    [property: JsonPropertyName("text")] string Text,
    [property: JsonPropertyName("to")] string To
);

public record MailListItem(
    [property: JsonPropertyName("attachment_count")] int AttachmentCount,
    [property: JsonPropertyName("first_attachment_name")] string FirstAttachmentName,
    [property: JsonPropertyName("from_mail")] string FromMail,
    [property: JsonPropertyName("from_name")] string FromName,
    [property: JsonPropertyName("is_new")] bool IsNew,
    [property: JsonPropertyName("mail_id")] int MailId,
    [property: JsonPropertyName("subject")] string Subject,
    [property: JsonPropertyName("time")] string Time
);

public record MailResponse(
    [property: JsonPropertyName("count")] int Count,
    [property: JsonPropertyName("first_id")] int FirstId,
    [property: JsonPropertyName("last_id")] int LastId,
    [property: JsonPropertyName("limit")] int Limit,
    [property: JsonPropertyName("more")] bool More,
    [property: JsonPropertyName("result")] bool Result,
    [property: JsonPropertyName("mail_list")] MailListItem[] MailList
);

internal record SecretAddressResponse(
    [property: JsonPropertyName("email")] string Email
);