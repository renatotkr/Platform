using System.Runtime.Serialization;

using Carbon.CI;
using Carbon.Versioning;

namespace Carbon.Platform.Computing
{
    public class ProgramDetails : IApplication, IProgramRelease
    {
        [DataMember(Name = "id", Order = 1)]
        public long Id { get; set; }

        [DataMember(Name = "name", Order = 2)]
        public string Name { get; set; }

        [DataMember(Name = "type", Order = 3)]
        public ProgramType Type { get; set; }

        [DataMember(Name = "version", Order = 4)]
        public SemanticVersion Version { get; set; }

        #region Details

        [DataMember(Name = "runtime", Order = 10)]
        public string Runtime { get; set; }

        [DataMember(Name = "urls", Order = 11)]
        public string[] Urls { get; set; }

        #endregion

        #region IRelease

        ReleaseType IRelease.Type => ReleaseType.Program;

        long IProgramRelease.ProgramId => Id;

        long IRelease.CommitId => 0;

        long IRelease.CreatorId => 0;

        #endregion
    }
}