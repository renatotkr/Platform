using System.Runtime.Serialization;

using Carbon.Versioning;

namespace Carbon.Platform.Computing
{
    public class ProgramDetails : IProgram
    {
        [DataMember(Name = "id", Order = 1)]
        public long Id { get; set; }

        [DataMember(Name = "name", Order = 2)]
        public string Name { get; set; }
        
        [DataMember(Name = "version", Order = 3)]
        public SemanticVersion Version { get; set; }

        [DataMember(Name = "type", Order = 4)]
        public ProgramType Type { get; set; }

        [DataMember(Name = "runtime", Order = 5)]
        public string Runtime { get; set; }

        [DataMember(Name = "addresses", Order = 6)]
        public string[] Addresses { get; set; }
    }
}