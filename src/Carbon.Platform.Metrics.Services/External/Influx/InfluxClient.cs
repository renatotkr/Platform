using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Carbon.Json;
using Carbon.Platform.Metrics;

namespace Influx
{
    public class InfluxClient
    {
        private readonly string endpoint;

        private readonly HttpClient http = new HttpClient {
            Timeout = TimeSpan.FromSeconds(10)
        };

        // http://ec2-34-201-167-255.compute-1.amazonaws.com:8086
        public InfluxClient(string endpoint)
        {
            this.endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
        }

        public async Task<InfluxQueryResponse> QueryAsync(InfluxQuery query)
        {
            using (var response = await http.GetAsync(endpoint + "/query" + query.ToQueryString()))
            {
                var responseText = await response.Content.ReadAsStringAsync();

                var json = JsonObject.Parse(responseText);

                if (json.TryGetValue("error", out var errorNode))
                {
                    throw new Exception(errorNode.ToString());
                }

                try
                {
                    return InfluxQueryResponse.FromJson(json);
                }
                catch(Exception ex)
                {
                    throw new Exception(responseText, ex);
                }
            }
        }

        /*
        public void CreateDatabase()
        {
        }

        public void DropDatabase()
        {
        }
        */

        public async Task<string> WriteAsync(string database, IList<MetricData> data)
        {
            if (data.Count == 0) return string.Empty;

            var sb = new StringBuilder();

            for (var i = 0; i < data.Count; i++)
            {
                if (i > 0) sb.Append('\n');

                data[i].WriteTo(sb);
            }

            // POST /write?db=mydb

            using (var response = await http.PostAsync(
                requestUri : endpoint + "/write?db=" + database, 
                content    : new StringContent(sb.ToString())))
            {
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}