using Carbon.Data.Annotations;
using Carbon.Data.Sequences;
using Carbon.Json;

namespace Carbon.Kms.Services
{
    public class CreateKeyGrantRequest
    {
        public Uid KeyId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public string[] Actions { get; set; }

        public JsonObject Constraints { get; set; }

        public JsonObject Properties { get; set; }

        public long UserId { get; set; }

        public string ExternalId { get; set; }
    }

    public static class KeyGrantPrivilege
    {
        public const string Encrypt = "encrypt";
        public const string Decrypt = "decrypt";
    }
}