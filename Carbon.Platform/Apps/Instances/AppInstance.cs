namespace Carbon.Platform
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    using Carbon.Data;

    // A machine may only have a single instance of an app running at a given point in time
    // This instance may be stopped, terminated, or change versions at anytime 

    [Table("AppInstances")]
    public class AppInstance : IAppInstance
    {
        public AppInstance() { }

        public AppInstance(IApp app, IMachine machine)
        {
            #region Preconditions

            if (app == null) throw new ArgumentNullException(nameof(app));
            if (machine == null) throw new ArgumentNullException(nameof(machine));

            #endregion

            // 15/2.5/18

            AppId     = app.Id;
            AppName   = app.Name;
            MachineId = machine.Id;
        }

        [Column("appId"), Key]
        [DataMember(Name = "appId")]
        public int AppId { get; set; }

        [Column("machineId"), Key]                  // Consider renaming instanceId
        [DataMember(Name = "machineId")]
        [Index("machineId-index")]
        public int MachineId { get; set; }

        [Column("host")] // Machine Host Name
        public string Host { get; set; }

        // Current Version
        [Column("appVersion")]
        public int AppVersion { get; set; }

        // Status
        [Column("status")]
        [Index("status")]
        public int? Status { get; set; }

        // Running, Stopped, Terminated

        [Column("started")]
        [DataMember(Name = "started")]
        public DateTime? Started { get; set; }

        [Column("terminated")]
        [DataMember(Name = "terminated")]
        public DateTime? Terminated { get; set; }

        [Column("modified")]
        [DataMember(Name = "modified")]
        public DateTime Modified { get; set; }

        [Column("appName")]
        public string AppName { get; set; }

        [Column("heartbeat")]
        public DateTime? Heartbeat { get; set; }

        #region Hosting

        [IgnoreDataMember]
        public object Site { get; set; }

        #endregion

        #region Helpers

        public string GetKey()
        {
            var buffer = new byte[12];

            var a = BitConverter.GetBytes(AppId);
            var v = BitConverter.GetBytes(AppVersion);
            var m = BitConverter.GetBytes(MachineId);

            Array.Copy(v, 0, buffer, 0, 4);
            Array.Copy(a, 0, buffer, 4, 4);
            Array.Copy(m, 0, buffer, 8, 4);

            return HexString.FromBytes(buffer);
        }

        public static AppInstance FromKey(string key)
        {
            var hex = HexString.ToBytes(key);

            var int1 = BitConverter.ToInt32(hex, 0);
            var int2 = BitConverter.ToInt32(hex, 4);
            var int3 = BitConverter.ToInt32(hex, 8);

            return new AppInstance
            {
                AppId = int2,
                AppVersion = int1,
                MachineId = int3
            };
        }

        public long GetId()
        {
            return AppTagId.Create(AppId, MachineId);
        }

        #endregion

    }
}

// PATH
// portfolio-manager/S19/0

// Mark terminated on termination