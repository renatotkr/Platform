using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Carbon.Libraries
{
    using Versioning;

    [Table("Libraries")]
    public class Library : ILibrary
    {
        public Library() { }

        public Library(string name, SemanticVersion version)
        {
            Name = name;
            Version = version;
        }

        [Column("name"), Key]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [Column("version"), Key]
        [DataMember(Name = "version")]
        public SemanticVersion Version { get; set; }

        [Column("source")]
        public string Source { get; set; }

        #region Main 

        [IgnoreDataMember]
        public string MainName { get; set; }

        [IgnoreDataMember]
        public byte[] MainSha256 { get; set; }

        [DataMember(Name = "main")]
        public LibraryFile Main
            => new LibraryFile(MainName, MainSha256);

        #endregion

        #region Helpers

        public string Path
            => "libs/" + Name + "/" + Version.ToString() + "/" + MainName;

        public override string ToString() 
            => Name + "@" + Version;

        #endregion
    }
}