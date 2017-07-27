﻿using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Platform.Environments;

namespace Carbon.Platform.Computing
{
    public interface IHostService
    {
        Task<HostInfo> GetAsync(long id);

        Task<HostInfo> GetAsync(string name);

        Task<HostInfo> FindAsync(ResourceProvider provider, string resourceId);

        Task<HostInfo> RegisterAsync(RegisterHostRequest request);

        Task<IReadOnlyList<HostInfo>> ListAsync(long ownerId);

        Task<IReadOnlyList<HostInfo>> ListAsync(ICluster cluster);

        Task<IReadOnlyList<HostInfo>> ListAsync(IEnvironment environment);
    }
}