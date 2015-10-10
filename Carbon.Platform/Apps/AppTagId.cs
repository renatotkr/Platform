namespace Carbon.Platform
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit)]
    public struct AppTagId
    {
        [FieldOffset(0)]
        private int version;

        [FieldOffset(4)]
        private int appId;

        [FieldOffset(0)]
        private long value;

        #region Implict Conversions

        public static implicit operator Int64(AppTagId tag)
        {
            return tag.value;
        }

        #endregion

        public int AppId => appId;

        public int Version => version;

        public long Value => value;

        public static AppTagId Create(long value)
        {
            return new AppTagId {
                value = value
            };
        }

        public static AppTagId Create(int appId, int version)
        {
            return new AppTagId
            {
                appId = appId,
                version = version
            };
        }
    }

    // A tag, or snapshot of an app -- 
    // version + commit + package

    // major, minor, build, and revision 

    // A tag identifies a specific application release
}
