using System;

using Carbon.Data;

namespace Carbon.Platform.Metrics
{
    public class MetricsDb
    {
        public MetricsDb(IDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            
            Metrics          = new Dataset<Metric,           long>(context);
            MetricDimensions = new Dataset<MetricDimension, (long, string)>(context);
            Series           = new Dataset<Series,          long>(context);
            SeriesPoints     = new Dataset<SeriesPoint,     (long, long)>(context);
        }

        public IDbContext Context { get; }

        public Dataset<Metric, long>                    Metrics          { get; }
        public Dataset<MetricDimension, (long, string)> MetricDimensions { get; }
        public Dataset<Series, long>                    Series           { get; }
        public Dataset<SeriesPoint, (long, long)>       SeriesPoints     { get; }
    }
}