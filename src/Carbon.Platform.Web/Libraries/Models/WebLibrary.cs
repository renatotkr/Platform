using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;

using Carbon.Platform.VersionControl;

namespace Carbon.Platform.Web
{
    using Versioning;

    [Dataset("WebLibraries")]
    public class WebLibrary : ILibrary
    {
        public WebLibrary() { }

        public WebLibrary(string name, SemanticVersion version)
        {
            Name     = name ?? throw new ArgumentNullException(nameof(name));
            Version  = version;
        }

        [Member("name"), Key]
        [DataMember(Name = "name")]
        public string Name { get; }

        [Member("version"), Key]
        [DataMember(Name = "version")]
        public SemanticVersion Version { get; }

        [Member("source")]
        [IgnoreDataMember]
        public string Source { get; set; }

        [Member("commitId")]
        public long CommitId { get; set; }

        #region Main 

        [IgnoreDataMember]
        public string MainName { get; set; }

        [IgnoreDataMember]
        public byte[] MainSha256 { get; set; }

        [DataMember(Name = "main")]
        public LibraryFile Main => new LibraryFile(MainName, MainSha256);

        #endregion

        #region Helpers

        [Obsolete("Use MainPath")]
        [IgnoreDataMember]
        public string Path => $"libs/{Name}/{Version}/{MainName}";

        [IgnoreDataMember]
        public string MainPath => $"libs/{Name}/{Version}/{MainName}";

        public override string ToString() => Name + "@" + Version;

        #endregion
    }
}