using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Carbon.Platform.Web
{
    using Versioning;

    [Table("WebLibraries")]
    public class WebLibrary : ILibrary
    {
        public WebLibrary() { }

        public WebLibrary(string name, SemanticVersion version)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Version = version;
        }

        [Column("name"), Key]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [Column("version"), Key]
        [DataMember(Name = "version")]
        public SemanticVersion Version { get; set; }

        [Column("source")]
        [IgnoreDataMember]
        public string Source { get; set; }

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