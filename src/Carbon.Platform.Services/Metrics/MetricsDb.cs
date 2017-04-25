using System;

using Carbon.Data;

namespace Carbon.Platform.Metrics
{
    using Metrics;

    public class MetricsDb
    {
        public MetricsDb(IDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            
            Metrics          = new Dataset<MetricInfo,      long>(context);
            MetricDimensions = new Dataset<MetricDimension, (long, string)>(context);
        }

        public IDbContext Context { get; }

        public Dataset<MetricInfo, long>                 Metrics  { get; }
        public Dataset<MetricDimension, (long, string)>  MetricDimensions { get; }

    }
}