using System;
using System.Runtime.Serialization;
using System.Text;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    [Dataset("Repositories")]
    [UniqueIndex("providerId", "fullName")]
    [UniqueIndex("ownerId", "name")]
    public class RepositoryInfo : IRepository
    {
        public RepositoryInfo() { }

        public RepositoryInfo(
            long id,
            string name, 
            long ownerId,
            ManagedResource resource)
        {
            #region Preconditions

            if (id <= 0)
                throw new ArgumentException("Must be > 0", nameof(id));

            #endregion

            Id         = id;
            Name       = name ?? throw new ArgumentNullException(nameof(name));
            FullName   = resource.ResourceId;
            OwnerId    = ownerId;
            ProviderId = resource.ProviderId;
            LocationId = resource.LocationId;
        }

        [Member("id"), Key(sequenceName: "repositoryId")]
        public long Id { get; }
        
        // namespace...

        [Member("name")]
        [StringLength(100)]
        public string Name { get; }

        [Member("fullName")]
        [StringLength(160)]
        public string FullName { get; }

        // Note: may change ownership
        [IgnoreDataMember]
        [Member("ownerId")] 
        [Indexed]
        public long OwnerId { get; }
        
        /// <summary>
        /// An encrypted token to access the repository
        /// </summary>
        [Member("encryptedToken")]
        [MaxLength(1000)]
        public byte[] EncryptedToken { get; }

        #region Statistics

        // Max ~4M
        [IgnoreDataMember]
        [Member("commitCount")]
        public int CommitCount { get; }

        #endregion

        #region IResource

        /*
        AWS CodeCommit : 1
        GitHub         : 1000
        BitBucket      : 1001
        GitLab         : 1002
        */

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        // e.g. carbon/cropper
        [StringLength(100)]
        string IManagedResource.ResourceId => FullName;
        
        // Used by aws:codecommit
        [IgnoreDataMember]
        [Member("locationId")]
        public int LocationId { get; }
        
        ResourceType IResource.ResourceType => ResourceTypes.Repository;

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
