using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carbon.Platform.Metrics;

namespace Influx
{
    public class InfluxDatabase
    {
        private readonly string name;
        private readonly InfluxClient client;

        public InfluxDatabase(string name, InfluxClient client)
        {
            this.name   = name ?? throw new ArgumentNullException(nameof(name));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public Task<InfluxQueryResponse> QueryAsync(string command)
        {
            var query = new InfluxQuery(name, command);

            return client.QueryAsync(query);
        }

        public Task<string> InsertAsync(MetricData data)
        {
            return client.WriteAsync(name, new[] { data });
        }

        public Task<string> InsertAsync(IList<MetricData> data)
        {
            return client.WriteAsync(name, data);
        }
    }
}