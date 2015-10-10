namespace Carbon.Platform
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    using Carbon.Data;

    // An app version is also a tag

    [Table("AppVersions")]
    public class AppVersion : IAppVersion
    {
        public AppVersion() { }

        public AppVersion(IApp app, int number)
        {
            #region Preconditions

            if (app == null) throw new ArgumentNullException("app");

            #endregion

            this.AppId = app.Id;
            this.Number = number;
        }

        [Column("appId"), Key]
        public int AppId { get; set; }

        [Column("num"), Key]
        public int Number { get; set; }

        [Column("commit")]
        public string Commit { get; set; }

        [Column("hash")]        // Package hash
        public byte[] Hash { get; set; }

        [Column("signature")]   // Package signature
        public byte[] Signature { get; set; }

        /*
		[Column("buildId")]
		public long? BuildId { get; set; }
		*/

        [Column("created")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime Created { get; set; }

        [Column("deployed")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Deployed { get; set; }

        [Column("activated")]
        [TimePrecision(TimePrecision.Second)]
        public DateTime? Activated { get; set; }

        #region Helpers

        public long GetKey()
        {
            return AppTagId.Create(AppId, Number).Value;
        }

        [IgnoreDataMember]
        public string HashHex
        {
            get { return HexString.FromBytes(Hash); }
        }

        #endregion
    }
}