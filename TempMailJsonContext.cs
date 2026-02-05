using System.Text.Json.Serialization;

namespace TempMailNET;

[JsonSerializable(typeof(MailResponse))]
[JsonSerializable(typeof(MailDetail))]
[JsonSerializable(typeof(SecretAddressResponse))]
internal partial class TempMailJsonContext : JsonSerializerContext
{
}