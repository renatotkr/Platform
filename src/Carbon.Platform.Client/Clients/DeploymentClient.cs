﻿using System.Threading.Tasks;

using Carbon.Data.Expressions;

namespace Carbon.Platform
{
    public class DeploymentClient
    {
        private readonly ApiBase api;

        internal DeploymentClient(ApiBase api)
        {
            this.api = api;
        }

        public Task<DeploymentDetails[]> ListAsync(Expression filter = null)
        {
            return api.GetListAsync<DeploymentDetails>($"/deployments" + filter?.ToQueryString());
        }

        public Task<DeploymentDetails> GetAsync(long id)
        {
            return api.GetAsync<DeploymentDetails>($"/deployments/{id}");
        }

        public Task<DeploymentDetails> CreateAsync(DeploymentDetails data)
        {
            return api.PostAsync<DeploymentDetails>($"/deployments", data);
        }
    }
}