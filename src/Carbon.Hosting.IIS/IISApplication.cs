using System;
using Carbon.Platform.Computing;

namespace Carbon.Hosting.IIS
{
    using Versioning;

    public class IISApplication : IApplication
    {
        public IISApplication(long id, string name, SemanticVersion version)
        {
            Id      = id;
            Name    = name ?? throw new ArgumentNullException(nameof(name));
            Version = version;
        }

        public long Id { get; }

        public string Name { get; }
         
        public SemanticVersion Version { get; }
    }
}