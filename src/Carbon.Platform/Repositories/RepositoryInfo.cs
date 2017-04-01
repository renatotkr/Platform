using System;
using System.Runtime.Serialization;
using System.Text;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Repositories
{
    [Dataset("Repositories")]
    public class RepositoryInfo : IManagedResource
    {
        // OwnerId + Sequence...

        [Member("id"), Key]
        public long Id { get; set; }
        
        [Member("name")]
        public string Name { get; set; }
        
        #region IResource

        // github
        [Member("providerId")]
        public int ProviderId { get; set; }

        // e.g. carbon/cropper
        [Member("resourceId")]
        [StringLength(100)]
        public string ResourceId { get; set; }

        ResourceType IManagedResource.Type => ResourceType.Repository;

        #endregion
        
        #region Timestamps

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

            var provider = ResourceProvider.Get(ProviderId);

            if (provider.Id != ResourceProvider.GitHub.Id)
            {
                sb.Append(provider.Domain);
                sb.Append(":");
            }

            sb.Append(ResourceId);

            return sb.ToString();
        }
    }
}
