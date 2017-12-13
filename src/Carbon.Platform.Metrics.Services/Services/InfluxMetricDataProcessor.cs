using System.Collections.Generic;
using System.Threading.Tasks;

using Influx;

namespace Carbon.Platform.Metrics
{
    public class InfluxMetricDataProcessor : IMetricDataProcessor
    {
        private readonly InfluxDatabase db;

        public InfluxMetricDataProcessor(InfluxDatabase db)
        {
            this.db = db;
        }

        public async Task ProcessAsync(MetricData data)
        {
            // TODO: Get the metric?

            await db.InsertAsync(data);
        }

        public async Task ProcessAsync(IList<MetricData> datas)
        {
            Validate.NotNull(datas, nameof(datas));

            await db.InsertAsync(datas);
        }
    }
}