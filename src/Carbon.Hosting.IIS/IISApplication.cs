using System;
using Carbon.Platform.Computing;

namespace Carbon.Hosting.IIS
{
    using Versioning;

    public class IISApplication : IProgram
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

        public string Runtime => "net470";

        public long? RepositoryId => null;
        
        public string[] Addresses => Array.Empty<string>();
    }
}