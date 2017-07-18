using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Sequences;
using Carbon.Platform.Storage;

namespace Carbon.Platform.Web
{
    using Versioning;

    [Dataset("WebLibraries")]
    public class WebLibrary : IWebLibrary
    {
        public WebLibrary() { }

        public WebLibrary(string name, SemanticVersion version)
        {
            Name     = name ?? throw new ArgumentNullException(nameof(name));
            Version  = version;
        }

        public WebLibrary(string name, SemanticVersion version, long commitId)
        {
            #region Preconditions
            
            if (commitId <= 0)
                throw new ArgumentException("Must be > 0", nameof(commitId));

            #endregion

            Name     = name ?? throw new ArgumentNullException(nameof(name));
            Version  = version;
            CommitId = commitId;
        }

        [Member("name"), Key]
        [DataMember(Name = "name")]
        [StringLength(100)]
        public string Name { get; }

        [Member("version"), Key]
        [DataMember(Name = "version")]
        [StringLength(20)]
        public SemanticVersion Version { get; }

        // TODO: Replace with commitId
        [Member("source")]
        [IgnoreDataMember]
        [StringLength(200)]
        public string Source { get; set; }

        [Member("commitId")]
        public long CommitId { get; }

        public long RepositoryId => ScopedId.GetScope(CommitId);
        
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