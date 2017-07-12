using System.Runtime.Serialization;

using Carbon.Versioning;

namespace Carbon.Platform.Computing
{
    public class ProgramDetails : IProgram
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
        
        [DataMember(Name = "version")]
        public SemanticVersion Version { get; set; }

        [DataMember(Name = "type")]
        public ProgramType Type { get; set; }

        [DataMember(Name = "runtime")]
        public string Runtime { get; set; }

        [DataMember(Name = "addresses")]
        public string[] Addresses { get; set; }
    }
}