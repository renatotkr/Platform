using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;

namespace Carbon.Platform.Web
{   
    [Dataset("WebComponents")]
    [DataIndex(IndexFlags.Unique, "namespace", "name", "version")]
    public class WebComponent : IWebComponent
    {
        public WebComponent() { }

        public WebComponent(long id, string ns, string name)
        {
            Id        = id;
            Namespace = ns ?? throw new ArgumentNullException(nameof(ns));
            Name      = name ?? throw new ArgumentNullException(nameof(name));
        }

        [Member("id"), Key(sequenceName: "webComponentId")] 
        public long Id { get; }

        [Member("namespace")]
        [StringLength(63)]
        public string Namespace { get; }

        // Element Name
        [Member("name"), Unique]
        [StringLength(63)]
        public string Name { get; }
        
        [Member("version"), Mutable]
        [StringLength(40)]
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
        public DateTime Modified { get; }

        #endregion

        // <carbon-gallary />
        // <carbon:gallery />
    }
}