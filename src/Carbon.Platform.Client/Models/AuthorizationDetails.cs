using System.Runtime.Serialization;
using Carbon.Data.Sequences;

namespace Carbon.Platform
{
    public class AuthorizationDetails
    {
        [DataMember(Name = "id")]
        public Uid Id { get; }

        // pending, valid, invalid, ...
        [DataMember(Name = "status")]
        public string Status { get; set; }
        
    }


    
    // POST /authorize
}