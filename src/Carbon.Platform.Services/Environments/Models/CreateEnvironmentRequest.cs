using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Environments
{
    [DataContract]
    public class CreateEnvironmentRequest
    {
        public CreateEnvironmentRequest() { }

        public CreateEnvironmentRequest(string name, long ownerId)
        {
            #region Preconditions

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Required", nameof(name));

            if (ownerId <= 0)
                throw new ArgumentException("Must be > 0", nameof(ownerId));

            #endregion

            Name    = name;
            OwnerId = ownerId;
        }

        [DataMember(Name = "name", Order = 1)]
        public string Name { get; set; }

        [DataMember(Name = "ownerId", Order = 2)]
        public long OwnerId { get; set; }
    }
}