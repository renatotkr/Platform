using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Time;

namespace Carbon.Platform.Metrics
{
    public class MetricDataProcessor : IMetricDataProcessor
    {
        private readonly IMetricService metricService;
        private readonly ISeriesService seriesService;
        private readonly ISeriesPointStore pointStore;

        public MetricDataProcessor(
            IMetricService metricService, 
            ISeriesService seriesService, 
            ISeriesPointStore pointStore)
        {
            this.metricService = metricService ?? throw new ArgumentNullException(nameof(metricService));
            this.seriesService = seriesService ?? throw new ArgumentNullException(nameof(seriesService));
            this.pointStore    = pointStore    ?? throw new ArgumentNullException(nameof(pointStore));
        }

        public async Task ProcessAsync(MetricData data)
        {
            // TODO: Get the metric & normalize the value if its not the base unit...
            

            var alignedTimestamp = AlignToGranularity(data.Timestamp ?? new Timestamp(DateTime.UtcNow));

            var seriesNames = Aggregates.GetSeriesPermutations(data);

            var points = new List<SeriesPoint>();

            foreach (var name in seriesNames)
            {
                var series = await seriesService.GetAsync(name);

                points.Add(new SeriesPoint(series.Id, alignedTimestamp, data.Value));
            }
            
            await pointStore.IncrementAsync(points);
        }

        public async Task ProcessAsync(IList<MetricData> datas)
        {
            Validate.NotNull(datas, nameof(datas));

            if (datas.Count == 0) return;

            var points = new List<SeriesPoint>();

            foreach (var data in datas)
            {
                var alignedTimestamp = AlignToGranularity(data.Timestamp ?? new Timestamp(DateTime.UtcNow));

                foreach (var seriesName in Aggregates.GetSeriesPermutations(data))
                {
                    var series = await seriesService.GetAsync(seriesName);

                    points.Add(new SeriesPoint(series.Id, alignedTimestamp, data.Value));
                }
            }

            await pointStore.IncrementAsync(points);
        }
        
        private static long AlignToGranularity(long ts, TimeUnit unit = TimeUnit.Minute)
        {
            return new Timestamp(new Timestamp(ts).DateTime.ToPrecision(unit)).Value;
        }
    }
}

// Granularity: the scale or level of detail present in a set of data or other phenomenon.