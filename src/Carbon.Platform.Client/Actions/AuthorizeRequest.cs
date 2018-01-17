using System.Runtime.Serialization;

namespace Carbon.Platform
{
    public class AuthorizeRequest
    {
        public AuthorizeRequest() { }

        public AuthorizeRequest(string email, string clientId)
        {
            Validate.NotNullOrEmpty(email, nameof(email));
            Validate.NotNull(clientId, nameof(clientId));
            Email = email;
            ClientId = clientId;
        }

        [DataMember(Name = "email")]
        public string Email { get; }

        [DataMember(Name = "clientId")]
        public string ClientId { get; }
    }
}