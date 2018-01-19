﻿using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Platform.Environments;

namespace Carbon.Platform.Computing
{
    public interface IClusterService
    {
        Task<IReadOnlyList<Cluster>> ListAsync(IEnvironment env);

        Task<Cluster> CreateAsync(CreateClusterRequest request);

        Task<Cluster> GetAsync(IEnvironment env, ILocation location);

        Task<Cluster> GetAsync(long id);

        Task<bool> DeleteAsync(ICluster cluster);
    }
}