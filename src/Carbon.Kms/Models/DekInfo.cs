﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Carbon.Data.Annotations;
using Carbon.Json;

namespace Carbon.Kms
{
    [Dataset("Deks", Schema = "Kms")]
    public class DekInfo : IKeyInfo
    {
        public DekInfo() { }

        public DekInfo(
            long id,
            long kekId,
            byte[] ciphertext,
            JsonObject context,
            string slug = null,
            int version = 1,
            KeyStatus status = KeyStatus.Active)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Invalid", nameof(id));

            if (kekId <= 0)
                throw new ArgumentException("Invalid", nameof(kekId));

            if (ciphertext == null || ciphertext.Length == 0)
                throw new ArgumentException("Required", nameof(ciphertext));

            #endregion

            Id         = id;
            Ciphertext = ciphertext;
            Context    = context;
            KekId      = kekId;
            Slug       = slug;
            Version    = 1;
            Status     = status;
        }

        [Member("id"), Key]
        public long Id { get; }

        [Member("version"), Key]
        public int Version { get; }

        // Key Encryption Key Id : The key used to decrypt the DEK data
        [Member("kekId")]
        public long KekId { get; }

        [Member("status")]
        public KeyStatus Status { get; }

        [Member("ciphertext"), MaxLength(1000)]
        public byte[] Ciphertext { get; }

        // scope, subject, etc
        [Member("context")]
        [StringLength(1000)]
        public JsonObject Context { get; }

        [Member("slug"), Unique]
        public string Slug { get; set; }

        #region Timestamps

        [Member("activated")]
        public DateTime? Activated { get; }

        [Member("accessed")]
        public DateTime? Accessed { get; }

        [Member("expires")]
        public DateTime? Expires { get; }

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }

        #endregion

        #region IDek

        string IKeyInfo.Id => Id.ToString();

        public IEnumerable<KeyValuePair<string, string>> GetAuthenticatedData()
        {
            if (Context == null) yield break;

            foreach (var property in Context)
            {
                yield return new KeyValuePair<string, string>(property.Key, property.Value.ToString());
            }
            
        }
    
        #endregion
    }
}

// Encrypted Data Key
// Keys should be destroyed shortly after expiration