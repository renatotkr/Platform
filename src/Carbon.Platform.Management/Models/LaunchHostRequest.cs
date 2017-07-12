using System;

using Carbon.Platform.Computing;

namespace Carbon.Platform.Management
{
    public class LaunchHostRequest
    {
        public LaunchHostRequest(Cluster cluster, ILocation location, HostTemplate template, int launchCount = 1)
        {
            #region Preconditions

            if (launchCount <= 0)
                throw new ArgumentException("Must be > 0", nameof(launchCount));

            #endregion

            Cluster     = cluster  ?? throw new ArgumentNullException(nameof(cluster));
            Location    = location ?? throw new ArgumentNullException(nameof(location));
            Template    = template ?? throw new ArgumentNullException(nameof(template));
            LaunchCount = launchCount;
        }

        public HostTemplate Template { get; }

        public Cluster Cluster { get; }

        public ILocation Location { get; }

        public int LaunchCount { get; }

        public string StartupScript { get; set; }
    }
}