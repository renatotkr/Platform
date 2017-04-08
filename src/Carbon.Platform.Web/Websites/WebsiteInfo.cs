using System;
using System.Runtime.Serialization;
using Carbon.Platform.VersionControl;

namespace Carbon.Platform.Web
{
    using Data.Annotations;
    
    [Dataset("Websites")]
    public class WebsiteInfo
    {
        public WebsiteInfo() { }

        public WebsiteInfo(long id, string name, long repositoryId, long ownerId)
        {
            Id           = id;
            Name         = name ?? throw new ArgumentNullException(nameof(name));
            RepositoryId = repositoryId;
            OwnerId      = ownerId;
        }

        [Member("id"), Key] 
        public long Id { get; }

        [Member("name"), Unique]
        [StringLength(63)]
        public string Name { get; }

        [Member("ownerId")] 
        public long OwnerId { get; }

        [Member("repositoryId")]
        public long RepositoryId { get; }

        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }
        
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; }

        #endregion

        #region IResource

        [Member("providerId")]
        public int ProviderId { get; set; } // Borg | Carbonmade

        // Type => ResourceType.Website;

        #endregion

        public override string ToString() => Name;
    }
}


// Notes:
// Themes are websites too
// Braches should have their own website

// TODO: Verify repository head format
// https://git-scm.com/docs/git-check-ref-format
