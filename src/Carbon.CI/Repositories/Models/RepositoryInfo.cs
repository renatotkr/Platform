using System;

using Carbon.Data.Annotations;
using Carbon.Json;
using Carbon.Platform.Resources;

namespace Carbon.CI
{
    [Dataset("Repositories", Schema = CiadDb.Name)]
    [UniqueIndex("ownerId", "name")]
    public class RepositoryInfo : IRepository, IResource
    {
        public RepositoryInfo() { }

        public RepositoryInfo(
            long id,
            string name, 
            long ownerId,
            string origin,
            int providerId,
            byte[] encryptedAcessToken = null,
            JsonObject properties = null)
        {
            Ensure.IsValidId(id);
            Ensure.IsValidId(ownerId, nameof(ownerId));
            Ensure.NotNullOrEmpty(name, nameof(name));
            
            Id                   = id;
            Name                 = name;
            Origin               = origin;
            OwnerId              = ownerId;
            ProviderId           = providerId;
            EncryptedAccessToken = encryptedAcessToken;
            Properties           = properties;
        }

        [Member("id"), Key(sequenceName: "repositoryId")]
        public long Id { get; }

        [Member("ownerId")]
        public long OwnerId { get; }

        [Member("name")]
        [StringLength(100)]
        public string Name { get; }

        [Member("providerId")]
        public int ProviderId { get; }

        [Member("origin"), Indexed]
        [StringLength(160)]
        public string Origin { get; }

        [Member("encryptedAccessToken")]
        [MaxLength(1000)]
        public byte[] EncryptedAccessToken { get; }

        [Member("properties")]
        [StringLength(1000)]
        public JsonObject Properties { get; }

        #region Statistics

        [Member("branchCount")]
        public int BranchCount { get; }

        // Max ~4M
        [Member("commitCount")]
        public int CommitCount { get; }

        #endregion

        #region IResource
        
        ResourceType IResource.ResourceType => ResourceTypes.Repository;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("deleted")]
        public DateTime? Deleted { get; }

        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion

        public override string ToString()
        {
            return Origin;
        }
    }
}
