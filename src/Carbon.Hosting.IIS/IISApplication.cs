using System;
using Carbon.Platform.Computing;

namespace Carbon.Hosting.IIS
{
    using Versioning;

    public class IISApplication : IProgram
    {
        public IISApplication(long id, string name, SemanticVersion version)
        {
            #region Preconditions

            if (id < 0)
                throw new ArgumentException("Must be >= 0", nameof(id));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Required", nameof(name));
            
            #endregion

            Id      = id;
            Name    = name;
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