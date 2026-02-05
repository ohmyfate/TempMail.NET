using System.Net.Http.Json;

namespace TempMailNET;

public class TempMailClient
{
    public const string MailtoPlus = "mailto.plus";
    public const string FexpostCom = "fexpost.com";
    public const string FexboxOrg = "fexbox.org";
    public const string MailboxInUa = "mailbox.in.ua";
    public const string RoverInfo = "rover.info";
    public const string ChitthiIn = "chitthi.in";
    public const string FextempCom = "fextemp.com";
    public const string AnyPink = "any.pink";
    public const string MerepostCom = "merepost.com";

    private static readonly HashSet<string> _validDomains = new(StringComparer.OrdinalIgnoreCase)
    {
        MailtoPlus, FexpostCom, FexboxOrg, MailboxInUa, 
        RoverInfo, ChitthiIn, FextempCom, AnyPink, MerepostCom
    };

    private readonly HttpClient _httpClient;
    public string Alias { get; }
    public string Domain { get; }

    private static readonly HttpClient _defaultClient = new();

    public TempMailClient(string alias, string domain, HttpClient? httpClient = null)
    {
        if (string.IsNullOrWhiteSpace(alias)) throw new ArgumentException(null, nameof(alias));
        if (!_validDomains.Contains(domain)) throw new ArgumentException(null, nameof(domain));

        Alias = alias;
        Domain = domain;
        _httpClient = httpClient ?? _defaultClient;
    }

    public override string ToString() => $"{Alias}@{Domain}";

    public async Task<MailResponse?> GetMailsAsync(CancellationToken ct = default)
    {
        var url = $"https://tempmail.plus/api/mails?email={Alias}@{Domain}&limit=20&epin=";
        try 
        {
            return await _httpClient.GetFromJsonAsync(url, TempMailJsonContext.Default.MailResponse, ct);
        }
        catch
        {
            return null;
        }
    }

    public async Task<MailDetail?> GetMailAsync(int id, CancellationToken ct = default)
    {
        var url = $"https://tempmail.plus/api/mails/{id}?email={Alias}@{Domain}&epin=";
        try
        {
            return await _httpClient.GetFromJsonAsync(url, TempMailJsonContext.Default.MailDetail, ct);
        }
        catch
        {
            return null;
        }
    }

    public async Task<string> GetSecretAddressAsync(CancellationToken ct = default)
    {
        var url = $"https://tempmail.plus/api/box/hidden?email={Alias}@{Domain}&epin=";
        try
        {
            var res = await _httpClient.GetFromJsonAsync(url, TempMailJsonContext.Default.SecretAddressResponse, ct);
            return res?.Email ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    public string GetAttachmentLink(int attachmentId, int mailId, Attachment[] attachments)
    {
        foreach (var att in attachments)
        {
            if (att.AttachmentId == attachmentId)
            {
                return $"https://tempmail.plus/api/mails/{mailId}/attachments/{attachmentId}?email={Alias}@{Domain}&epin=";
            }
        }
        return string.Empty;
    }
}