using System;

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
            SemanticVersion version,
            JsonObject properties = null,
            string runtime = null,
            string[] addresses = null,
            ProgramType type = ProgramType.App,
            long? repositoryId = null,
            long? parentId = null
        )
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Required", nameof(name));
             
            if (ownerId <= 0)
                throw new ArgumentException("Must be > 0", nameof(ownerId));

            #endregion

            Id           = id;
            Name         = name;
            Slug         = slug;
            Type         = type;
            Runtime      = runtime;
            Addresses    = addresses;
            Properties   = properties ?? new JsonObject();
            OwnerId      = ownerId;
            Version      = version;
            RepositoryId = repositoryId;
            ParentId     = parentId;
        }
        
        [Member("id"), Key(sequenceName: "programId")]
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

        // a site is "run" by an application
        [Member("parentId")]
        public long? ParentId { get; }

        [Member("repositoryId")]
        public long? RepositoryId { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region Stats

        [Member("releaseCount")]
        public int ReleaseCount { get; }

        #endregion

        #region IResource

        ResourceType IResource.ResourceType => ResourceTypes.Program;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}