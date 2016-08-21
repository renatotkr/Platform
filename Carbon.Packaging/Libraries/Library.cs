using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Carbon.Platform
{
    using Data;
    using Data.Annotations;

    [Record(TableName = "Libraries")]
    public class Library
    {
        public Library() { }

        public Library(string name, Semver version)
        {
            Name = name;
            Version = version;
        }

        [Key]
        public string Name { get; set; }

        [Key]
        public Semver Version { get; set; }

        [Unique]
        public CryptographicHash Hash { get; set; }
        
        public long DeployerId { get; set; }

        public long RepositoryId { get; }

        [Version(false)]
        public DateTime Created { get; set; }

        #region Maps

        [Exclude, IgnoreDataMember]
        public IList<Library> Dependencies { get; set; }

        [Exclude, IgnoreDataMember]
        internal bool IsResolved { get; set; }

        [Exclude]
        public IList<LibraryFile> Files { get; set; }

        #endregion

        public override string ToString()
            => Name + "@" + Version;
    }

    [Record(TableName = "LibraryFiles")]
    public class LibraryFile
    {
        [Key]
        public long LibraryId { get; set; }

        [Key]
        public string Name { get; set; }
    }
}