using System;

namespace Carbon.Platform.Security
{
    public interface IEncryptionKeyInfo
    {
        long Id { get; }
      
        int Version { get; }

        DateTime? NextRotation { get; }

        long LocationId { get; }
    }
}