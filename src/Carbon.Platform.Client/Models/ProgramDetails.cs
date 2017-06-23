﻿using System.Runtime.Serialization;

using Carbon.Versioning;

namespace Carbon.Platform.Computing
{
    public class ProgramDetails : IApplication, IProgramRelease
    {
        [DataMember(Name = "id", Order = 1)]
        public long Id { get; set; }

        [DataMember(Name = "name", Order = 2)]
        public string Name { get; set; }
        
        [DataMember(Name = "version", Order = 4)]
        public SemanticVersion Version { get; set; }

        [DataMember(Name = "type", Order = 3)]
        public ProgramType Type { get; set; }

        [DataMember(Name = "runtime", Order = 4)]
        public string Runtime { get; set; }

        #region Details

        [DataMember(Name = "urls", Order = 11)]
        public string[] Urls { get; set; }

        #endregion

        #region IRelease

        long IProgramRelease.ProgramId => Id;

        long IProgramRelease.CommitId => 0;

        long IProgramRelease.CreatorId => 0;

        #endregion
    }
}