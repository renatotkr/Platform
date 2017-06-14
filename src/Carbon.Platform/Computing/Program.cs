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
    public class Program : IApplication, IResource
    {
        public Program() { }

        public Program(
            long id, 
            string name,
            string slug, 
            long ownerId,
            JsonObject properties = null,
            ProgramType type = ProgramType.Application
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
        [StringLength(63)]
        public string Name { get; }
        
        // e.g. accelerator | ngnix
        [Member("slug"), Unique]
        [StringLength(63)]
        public string Slug { get; }

        [Member("version")]
        public SemanticVersion Version { get; }
        
        [Member("runtime")]
        [StringLength(50)]
        public string Runtime { get; set; }

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

        #region Properties

        string[] IApplication.Urls
        {
            get => (Properties.TryGetValue("urls", out var addresses))
                ? addresses.ToArrayOf<string>()
                : null;
        }

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
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion
    }
}