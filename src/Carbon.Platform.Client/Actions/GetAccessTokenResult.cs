using System.Runtime.Serialization;

namespace Carbon.Platform
{
    public class GetAccessTokenResult
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        [DataMember(Name = "refresh_token")]
        public string RefreshToken { get; set; }

        [DataMember(Name = "expires_in")]
        public int ExpiresIn { get; set; }
    }
    
    // POST /authorize
}