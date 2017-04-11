using System;
using Carbon.Platform.Apps;

namespace Carbon.Hosting.IIS
{
    using Versioning;

    public class IISApplication : IApp
    {
        public IISApplication(long id, string name, SemanticVersion version, DateTime created)
        {
            Id      = id;
            Name    = name;
            Version = version;
            Created = created;
        }

        public long Id { get; }

        public string Name { get; }
         
        public SemanticVersion Version { get; }

        public DateTime Created { get; }
    }

}