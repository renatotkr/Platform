using System;
using System.Runtime.Serialization;

namespace Carbon.Platform.Web
{
    public class LibraryFile
    {
        public LibraryFile() { }

        public LibraryFile(string name, byte[] sha256)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Sha256 = sha256;
        }
        
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "sha256")]
        public byte[] Sha256 { get; set; }
    }
}
