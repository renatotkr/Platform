using System.Runtime.Serialization;
using Carbon.Data.Sequences;

namespace Carbon.Platform
{
    public class AuthorizeResult
    {
        [DataMember(Name = "id")]
        public Uid Id { get; set; }
    }
    
    // POST /authorize
}