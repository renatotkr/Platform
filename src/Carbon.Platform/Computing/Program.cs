using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;
using Carbon.Versioning;

namespace Carbon.Platform.Computing
{
    [Dataset("Programs", Schema = "Computing")]
    [UniqueIndex("ownerId", "name")]
    public class ProgramInfo : IProgram, IResource
    {
        public ProgramInfo() { }

        public ProgramInfo(
            long id, 
            string name,
            string slug, 
            long ownerId,
            JsonObject properties = null,
            string runtime = null,
            string[] addresses = null,
            ProgramType type = ProgramType.App
        )
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (ownerId <= 0)
                throw new ArgumentException("Must be > 0", nameof(ownerId));

            #endregion

            Id         = id;
            Name       = name ?? throw new ArgumentNullException(nameof(name));
            Slug       = slug;
            Type       = type;
            Runtime    = runtime;
            Addresses  = addresses,
            Properties = properties;
            OwnerId    = ownerId;
        }

        [Member("id"), Key(sequenceName: "programId", increment: 4)]
        public long Id { get; }

        [Member("type")]
        public ProgramType Type { get; }

        [Member("ownerId")]
        public long OwnerId { get; }

        [Member("name")]
        [StringLength(100)]
        public string Name { get; }
        
        // e.g. accelerator | ngnix | caddy
        [Member("slug"), Unique]
        [StringLength(63)]
        public string Slug { get; }

        [Member("runtime")]
        [StringLength(50)]
        public string Runtime { get; }

        [Member("addresses")]
        [StringLength(200)]
        public string[] Addresses { get; }

        [Member("version")]
        public SemanticVersion Version { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region Stats

        [Member("releaseCount")]
        public int ReleaseCount { get; }

        #endregion

        #region Resource

        [Member("repositoryId")]
        public long RepositoryId { get; set; }

        #endregion

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.Program;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}