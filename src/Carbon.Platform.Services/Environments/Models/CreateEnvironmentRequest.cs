using System;

namespace Carbon.Platform.Environments
{
    public class CreateEnvironmentRequest
    {
        public CreateEnvironmentRequest() { }

        public CreateEnvironmentRequest(string name, long ownerId)
        {
            Name    = name ?? throw new ArgumentNullException(nameof(name));
            OwnerId = ownerId;
        }

        public string Name { get; set; }

        public long OwnerId { get; set; }
    }
}