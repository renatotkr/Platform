﻿namespace Carbon.CI
{
    public interface IRepository
    {
        long Id { get; }

        long OwnerId { get; }

        string Name { get; }     // platform

        int ProviderId { get; }

        string Origin { get; }   // carbon/platform

        byte[] EncryptedAccessToken { get; }
    }
}

// origin examples: 
// github:carbonmade/lefty
// bitbucket:carbonmade/lefty