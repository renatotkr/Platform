using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

using Carbon.Data;

namespace Carbon.Platform
{
    [Table("FrontendVersions")]
    public class FrontendVersion
    {
        public FrontendVersion() { }

        public FrontendVersion(IFrontend frontend, int number)
        {
            FrontendName = frontend.Name;
            Number = number.ToString();
        }

        [Key, Column("frontend")] // Rename Id
        public string FrontendName { get; set; }

        [Key, Column("name")] // TODO, change to a number type
        public string Number { get; set; }

        [Column("commit")]
        [StringLength(20)]
        public string Commit { get; set; }

        [Column("verified")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Verified { get; set; }

        [Column("flags")]
        public FrontendFlags Flags { get; set; }

        [Column("deployed")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deployed { get; set; }

        [Column("activated")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Activated { get; set; }

        [Column("created")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime Created { get; set; }

        [Column("creatorId")]
        public int CreatorId { get; set; }

        #region Helpers

        public bool Hotfix => Flags.HasFlag(FrontendFlags.Hotfix);

        [IgnoreDataMember]
        public object Creator { get; set; }

        [IgnoreDataMember]
        public string Path => FrontendName + "/" + Number;

        // lefty/1.0.2

        #endregion
    }

    [Flags]
    public enum FrontendFlags
    {
        None = 0,

        Hotfix = 1 << 5
    }
}
