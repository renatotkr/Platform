using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Web
{
    using Data.Annotations;
    
    [Dataset("WebComponents")]
    [DataIndex(IndexFlags.Unique, "namespace", "name", "version")]
    public class WebComponent
    {
        public WebComponent() { }

        public WebComponent(long id, string name)
        {
            Id   = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        [Member("id"), Key] 
        public long Id { get; set; }
        

        [Member("namespace")]
        [StringLength(63)]
        public string Namespace { get; set; }

        // Element Name
        [Member("name"), Unique]
        [StringLength(63)]
        public string Name { get; set; }

        [Member("version"), Mutable] // e.g. master, ...
        public string Version { get; set; }

        [Member("style")]
        [StringLength(2000)]
        public string Style { get; set; }

        [Member("script")]
        [StringLength(2000)]
        public string Script { get; set; }

        [Member("template")]
        [StringLength(2000)]
        public string Template { get; set; }

        [Member("ownerId")]
        public long OwnerId { get; set; }
        
        #region Timestamps

        [IgnoreDataMember]
        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [IgnoreDataMember]
        [Member("deleted")]
        public DateTime? Deleted { get; }
        
        [Member("modified"), Timestamp(true)]
        public DateTime Modified { get; set; }

        #endregion

        // <carbon-gallary />
        // <carbon:gallery />
    }
}