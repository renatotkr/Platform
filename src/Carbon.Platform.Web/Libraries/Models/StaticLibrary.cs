using System.Runtime.Serialization;

namespace Carbon.Platform.Web
{
    using Versioning;

    public class StaticLibrary : IWebLibrary
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "version")]
        public SemanticVersion Version { get; set; }

        [DataMember(Name = "main")]
        public LibraryFile Main { get; set; }

        #region Helpers

        [IgnoreDataMember]
        public string Path => $"libs/{Name}/{Version}/{Main.Name}";

        public override string ToString()
        {
            return Name + "@" + Version;
        }

        #endregion
    }
}