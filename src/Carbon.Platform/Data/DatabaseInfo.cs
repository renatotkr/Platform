﻿using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Data
{
    [Dataset("Databases")]
    public class DatabaseInfo : IDatabase
    {
        public DatabaseInfo() { }

        public DatabaseInfo(long id, string name)
        {
            Id   = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        [Member("id"), Key]
        public long Id { get; }

        [Member("name"), Unique]
        public string Name { get; }

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        #endregion

        #region IResource

        ResourceType IResource.ResourceType => ResourceType.Database;

        #endregion
    }
}