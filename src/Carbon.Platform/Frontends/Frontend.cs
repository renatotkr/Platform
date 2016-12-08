using System;

namespace Carbon.Platform.Frontends
{
    using Data.Annotations;
    using Versioning;

    [Dataset("Frontends")]
    public class Frontend : IFrontend
    {
        public Frontend() { }

        public Frontend(string name)
        {
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            #endregion

            Name = name;
        }

        [Member("id"), Identity] 
        public long Id { get; set; }

        [Member("version"), Mutable] // Active version
        public SemanticVersion Version { get; set; }

        [Member("name"), Unique]
        [StringLength(50)]
        public string Name { get; set; }

        [Member("source"), Mutable]  // e.g. carbonmade/lefty#commit/head
        [StringLength(100)]
        public string Source { get; set; }

        [Member("appId")]
        public long AppId { get; set; }

        [Member("modified"), Timestamp]
        public DateTime Modified { get; set; }

        [Member("created")]
        public DateTime Created { get; set; }

        public override string ToString() => Name + "@" + Version;  // lefty@1.0.2
    }
}