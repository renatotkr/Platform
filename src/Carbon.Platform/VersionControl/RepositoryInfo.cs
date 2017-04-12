using System;
using System.Runtime.Serialization;
using System.Text;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.VersionControl
{
    [Dataset("Repositories")]
    [DataIndex(IndexFlags.Unique, "providerId", "fullName")]
    [DataIndex(IndexFlags.Unique, "ownerId", "name")]
    public class RepositoryInfo : IRepository
    {
        public RepositoryInfo() { }

        public RepositoryInfo(long id, string name, long ownerId, ManagedResource resource)
        {
            Id         = id;
            Name       = name ?? throw new ArgumentNullException(nameof(name));
            FullName   = resource.ResourceId;
            OwnerId    = ownerId;
            ProviderId = resource.ProviderId;
            LocationId = resource.LocationId;
        }

        [Member("id"), Key]
        public long Id { get; }
        
        [Member("name")]
        public string Name { get; }

        [IgnoreDataMember]
        [Member("ownerId")]  // May change ownership
        public long OwnerId { get; }

        [Member("fullName")]
        [StringLength(100)]
        public string FullName { get; }

        #region Stats

        // Max ~4M
        [IgnoreDataMember]
        [Member("commitCount")]
        public int CommitCount { get; }

        #endregion

        #region IResource

        /*
        Amazon CodeCommit : 1
        GitHub            : 1000
        BitBucket         : 1001
        GitLab            : 1002
        */

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        // e.g. carbon/cropper
        [StringLength(100)]
        string IManagedResource.ResourceId => FullName;
        
        // Used by amazon codecommit
        [IgnoreDataMember]
        [Member("locationId")]
        public long LocationId { get; }
        
        ResourceType IResource.ResourceType => ResourceType.Repository;

        #endregion

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deleted { get; }

        #endregion
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            
            // bitbucket:carbonmade/lefty
            if (ProviderId != ResourceProvider.GitHub.Id)
            {
                var provider = ResourceProvider.Get(ProviderId);

                sb.Append(provider.Name);
                sb.Append(":");
            }

            sb.Append(FullName);

            return sb.ToString();
        }
    }
}
