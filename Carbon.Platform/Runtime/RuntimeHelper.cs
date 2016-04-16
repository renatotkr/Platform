using System;
using System.IO;

namespace Carbon.Platform
{
    public static class RuntimeHelper
    {
        private static AppInstance instance;

        private static string appName;
        private static string appVersion;

        private static readonly object _lock = new object();

        public static AppInstance GetAppInstance()
        {
            if (instance == null)
            {
                var fileName = GetInstanceFileName();

                if (!File.Exists(fileName)) return null;

                lock (_lock)
                {
                    if (instance == null)
                    {
                        var text = File.ReadAllText(fileName);

                        instance = AppInstance.FromKey(text);
                    }
                }
            }

            return instance;
        }

        public static string GetAppVersion()
        {
            if (appVersion == null)
            {
                var parts = AppDomain.CurrentDomain.BaseDirectory.Split(new[] { Path.DirectorySeparatorChar });

                appVersion = parts[parts.Length - 2];
            }

            return appVersion;
        }

        public static string GetAppName()
        {
            if (appName == null)
            {
                var parts = AppDomain.CurrentDomain.BaseDirectory.Split(new[] { Path.DirectorySeparatorChar });

                appName = parts[parts.Length - 3];
            }

            return appName;
        }

        #region Helpers

        private static string GetInstanceFileName()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            return Path.Combine(baseDirectory, "instance.txt");
        }

        #endregion
    }
}

/*
instance.txt
23000000030000002d000000
------------------------
app.toml
 
id        = 1
version   = master
machineId = 35
deployed  = "2014-04-01"
activated = "2014-04-01"

*/
