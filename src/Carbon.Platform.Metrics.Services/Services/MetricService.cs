using System;
using System.Collections.Concurrent;

using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Platform.Sequences;

namespace Carbon.Platform.Metrics
{
    using static Expression;

    public class MetricService : IMetricService
    {
        private readonly ConcurrentDictionary<string, Metric> cache = new ConcurrentDictionary<string, Metric>();

        private readonly MetricsDb db;

        public MetricService(MetricsDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<Metric> GetAsync(long id)
        {
            return await db.Metrics.FindAsync(id);
            // ?? throw ResourceError.NotFound(ResourceTypes.Metric, id);
        }

        public async Task<Metric> GetAsync(string name)
        {
            Ensure.NotNullOrEmpty(name, nameof(name));

            if (!cache.TryGetValue(name, out var metric))
            {
                metric = await db.Metrics.QueryFirstOrDefaultAsync(
                    And(Eq("name", name), IsNull("deleted"))
                );

                // create on the fly
                if (metric == null)
                {
                    metric = await CreateAsync(new CreateMetricRequest(name, 1, MetricType.Delta, "count", null));
                }

                cache.TryAdd(name, metric);
            }

            return metric;
        }

        public async Task<Metric> CreateAsync(CreateMetricRequest request)
        {
            Ensure.NotNull(request, nameof(request));

            var id = await db.Metrics.Sequence.NextAsync();

            var metric = new Metric(
                id      : id,
                ownerId : request.OwnerId,
                name    : request.Name,
                type    : request.Type,
                unit    : request.Unit
            );

            await db.Metrics.InsertAsync(metric);

            if (request.Dimensions != null)
            {
                var dimensions = new MetricDimension[request.Dimensions.Length];

                for (var i = 0; i < dimensions.Length; i++)
                {                    
                    dimensions[i] = new MetricDimension(
                        id       : ScopedId.Create(metric.Id, i + 1),
                        name     : request.Name
                    );
                }
                
                await db.MetricDimensions.InsertAsync(dimensions);
            }

            return metric;
        }
    }
}