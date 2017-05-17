using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    [Dataset("Programs")]
    public class Program : IProgram, IResource
    {
        public Program() { }

        public Program(long id, string name, string slug, long ownerId)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            #endregion

            Id      = id;
            Name    = name ?? throw new ArgumentNullException(nameof(name));
            Slug    = slug;
            Type    = ProgramType.Application;
            OwnerId = ownerId;
        }

        [Member("id"), Key(sequenceName: "programId", increment: 4)]
        public long Id { get; }
        
        [Member("name")]
        public string Name { get; }
        
        // e.g. accelerator | ngnix
        [Member("slug"), Unique]
        [StringLength(63)]
        public string Slug { get; }

        // Application, Service, Task
        [Member("type")]
        public ProgramType Type { get; set; }

        [Member("ownerId")]
        public long OwnerId { get; }

        #region Stats

        [Member("releaseCount")]
        public int ReleaseCount { get; }

        #endregion

        #region Resource

        [Member("repositoryId")]
        public long RepositoryId { get; set; }

        #endregion

        #region IResource

        ResourceType IResource.ResourceType => ResourceType.Program;

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

// Each program spans four envs: development, intergration, staging, and production
// The program's environment is responsible for exposing itself (i.e. to the web) or staying internal