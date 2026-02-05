# TempMailNET

A high-performance .NET client library for the tempmail.plus API.

## Installation

Add the `TempMailNET` library to your project references.

```bash
dotnet add package TempMailNET
```

```csharp
using TempMailNET;
```

## Usage

### 1. Initialization

Initialize the client with a username alias and a valid domain constant.

```csharp
// Standard initialization
var client = new TempMailClient("johndoe", TempMailClient.FexpostCom);

// Optional: Inject a shared HttpClient for connection pooling
var sharedHttpClient = new HttpClient();
var clientWithPool = new TempMailClient("johndoe", TempMailClient.FexpostCom, sharedHttpClient);
```

**Available Domains:**
*   `TempMailClient.MailtoPlus`
*   `TempMailClient.FexpostCom`
*   `TempMailClient.FexboxOrg`
*   `TempMailClient.MailboxInUa`
*   `TempMailClient.RoverInfo`
*   `TempMailClient.ChitthiIn`
*   `TempMailClient.FextempCom`
*   `TempMailClient.AnyPink`
*   `TempMailClient.MerepostCom`

### 2. Get Secret Address

Retrieve the hidden email address associated with the mailbox.

```csharp
string secretAddress = await client.GetSecretAddressAsync();
```

### 3. List Emails (Inbox)

Fetch the inbox to see the latest messages. This returns a `MailResponse` object containing the list of items.

```csharp
var inbox = await client.GetMailsAsync();

if (inbox != null)
{
    Console.WriteLine($"Total emails: {inbox.Count}");
    
    foreach (var item in inbox.MailList)
    {
        Console.WriteLine($"[{item.Time}] {item.FromName}: {item.Subject}");
    }
}
```

#### Model: `MailResponse`
The wrapper object returned by `GetMailsAsync`.

*   **Count** (`int`): Total number of emails returned.
*   **Result** (`bool`): API success status.
*   **More** (`bool`): Indicates if there are more emails on the server.
*   **Limit** (`int`): The limit applied to the request (default 20).
*   **FirstId** (`int`): The ID of the newest email.
*   **LastId** (`int`): The ID of the oldest email in this batch.
*   **MailList** (`MailListItem[]`): Array of email summaries (see below).

#### Model: `MailListItem`
Represents a single email summary in the inbox list.

*   **MailId** (`int`): Unique identifier for the email (used to fetch details).
*   **Subject** (`string`): The email subject line.
*   **Time** (`string`): The time received (e.g., "10:30").
*   **FromMail** (`string`): The sender's email address.
*   **FromName** (`string`): The sender's display name.
*   **IsNew** (`bool`): True if the email has not been read.
*   **AttachmentCount** (`int`): Number of attachments.
*   **FirstAttachmentName** (`string`): Filename of the first attachment, if any.

---

### 4. Read Email Details

Fetch the full content of a specific message using its ID. This returns a `MailDetail` object.

```csharp
var email = await client.GetMailAsync(12345); // ID from MailListItem

if (email != null)
{
    Console.WriteLine(email.Text); // Plain text body
    Console.WriteLine(email.Html); // HTML body
}
```

#### Model: `MailDetail`
Represents the full content of a specific email.

*   **MailId** (`int`): Unique identifier.
*   **MessageId** (`string`): The SMTP message-id header.
*   **Subject** (`string`): The email subject.
*   **Date** (`string`): Full date and time string.
*   **From** (`string`): Full sender string (Name <email>).
*   **FromName** (`string`): Sender's display name.
*   **FromMail** (`string`): Sender's email address.
*   **FromIsLocal** (`bool`): True if sender is within the same system.
*   **To** (`string`): Recipient address.
*   **Text** (`string`): Plain text body content.
*   **Html** (`string`): HTML body content.
*   **IsTls** (`bool`): True if received via TLS.
*   **Attachments** (`Attachment[]`): Array of attachment details (see below).
*   **Result** (`bool`): API success status.

---

### 5. Attachments

You can access attachment metadata via `MailDetail` and generate download links.

```csharp
if (email.Attachments.Length > 0)
{
    foreach (var att in email.Attachments)
    {
        string url = client.GetAttachmentLink(att.AttachmentId, email.MailId, email.Attachments);
        Console.WriteLine($"File: {att.Name} ({att.Size} bytes) -> {url}");
    }
}
```

#### Model: `Attachment`
Represents a file attached to an email.

*   **AttachmentId** (`int`): Unique identifier for the attachment.
*   **Name** (`string`): The filename (e.g., "document.pdf").
*   **Size** (`int`): File size in bytes.

## Error Handling

The library avoids throwing exceptions for runtime network errors.

*   `GetMailsAsync`: Returns `null` on failure.
*   `GetMailAsync`: Returns `null` on failure.
*   `GetSecretAddressAsync`: Returns `string.Empty` on failure.
*   **Constructor**: Throws `ArgumentException` if the alias is empty or domain is invalid.