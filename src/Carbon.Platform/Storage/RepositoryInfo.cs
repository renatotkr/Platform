using System;
using System.Runtime.Serialization;
using System.Text;

using Carbon.Data.Annotations;
using Carbon.Platform.Resources;

namespace Carbon.Platform.Storage
{
    [Dataset("Repositories", Schema = "Storage")]
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

        [Member("ownerId")]
        [IgnoreDataMember]
        public long OwnerId { get; }

        // Namespace?

        [Member("name")]
        [StringLength(100)]
        public string Name { get; }

        [Member("fullName")]
        [StringLength(160)]
        public string FullName { get; }

        [Member("encryptedAccessToken")]
        [MaxLength(1000)]
        public byte[] EncryptedAccessToken { get; set; }

        #region Statistics

        // Max ~4M
        [IgnoreDataMember]
        [Member("commitCount")]
        public int CommitCount { get; }

        #endregion

        #region IResource

        [IgnoreDataMember]
        [Member("providerId")]
        public int ProviderId { get; }

        // // e.g. carbon/cropper
        // string IManagedResource.ResourceId => FullName;
        
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
        [Member("deleted")]
        public DateTime? Deleted { get; }

        [IgnoreDataMember]
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

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
