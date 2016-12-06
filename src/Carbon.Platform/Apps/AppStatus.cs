using System;
using System.Collections.Generic;
using System.Linq;
namespace Carbon.Platform.Apps
{
    public enum AppStatus
    {
        Unknown   = 0,
        Pending   = 1, // provisioning
        Running   = 2,
        Draining  = 3, // not acceping new connections
        Stopped   = 4  // stopped
    }
}