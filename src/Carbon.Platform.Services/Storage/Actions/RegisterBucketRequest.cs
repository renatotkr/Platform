﻿using System;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    public class RegisterBucketRequest
    {
        public RegisterBucketRequest(string name, long ownerId, ManagedResource resource)
        {
            #region Validation

            Validate.NotNullOrEmpty(name, nameof(name));

            if (name.Length > 63)
                throw new ArgumentException("Must be 63 characters or fewer", nameof(name));

            Validate.Id(ownerId, nameof(ownerId));

            #endregion

            Name     = name;
            Resource = resource;
            OwnerId  = ownerId;
        }

        public string Name { get; }
        
        public long OwnerId { get; }

        public ManagedResource Resource { get; }
    }
}