using System.Runtime.Serialization;

namespace Carbon.Platform
{
    public class AuthorizeRequest
    {
        public AuthorizeRequest() { }

        public AuthorizeRequest(string email, string clientId)
        {
            Ensure.NotNullOrEmpty(email, nameof(email));
            Ensure.NotNull(clientId, nameof(clientId));

            Email = email;
            ClientId = clientId;
        }

        [DataMember(Name = "email")]
        public string Email { get; }

        [DataMember(Name = "clientId")]
        public string ClientId { get; }
    }
}