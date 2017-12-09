using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Storage
{
    [DataContract]
    public class RepositoryCommitDetails
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "sha1", EmitDefaultValue = false)]
        public byte[] Sha1 { get; set; }

        [DataMember(Name = "message", EmitDefaultValue = false)]
        public string Message { get; set; }

        [DataMember(Name = "created", EmitDefaultValue = false)]
        public DateTime? Created { get; set; }
    }
}
