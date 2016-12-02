using System;
using System.Runtime.Serialization;

namespace Carbon.Libraries
{
    public struct LibraryFile
    {
        public LibraryFile(string name, byte[] sha256)
        {
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            #endregion

            Name = name;
            Sha256 = sha256;
        }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "sha256")]
        public byte[] Sha256 { get; set; }
    }
}
