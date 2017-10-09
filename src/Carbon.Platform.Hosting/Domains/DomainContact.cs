/*
using System;
using Carbon.Data.Annotations;
using Carbon.Json;

namespace Carbon.Platform.Hosting
{
    [Dataset("DomainContacts")]
    public class DomainContact
    {
        public DomainContact() { }

        public DomainContact(
            long id,
            long domainId,
            string name,
            JsonObject properties = null)
        {
            Id         = id;
            DomainId   = domainId;
            Name       = name;
            Properties = properties ?? new JsonObject();
        }

        [Member("id"), Key(sequenceName: "domainContactId")]
        public long Id { get; }
        
        [Member("domainId")]
        public long DomainId { get; }
        
        [Member("entityId")]
        public long EntityId { get; set; }

        [Member("name")]
        [Ascii, StringLength(253)]
        public string Name { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region Timestamps

        [Member("validated")]
        public DateTime? Validated { get; }

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}
*/