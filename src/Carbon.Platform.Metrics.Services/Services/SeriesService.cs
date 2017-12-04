using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

using Carbon.Data.Expressions;
using Carbon.Time;

namespace Carbon.Platform.Metrics
{
    using static Expression;

    public class SeriesService : ISeriesService
    {
        private readonly MetricsDb db;

        private readonly ConcurrentDictionary<(string, string), Series> cache = new ConcurrentDictionary<(string, string), Series>();
        
        public SeriesService(MetricsDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        // Series objects are immutable -- consider caching here.

        public async ValueTask<Series> GetAsync(long id)
        {
            return await db.Series.FindAsync(id);
               // ?? throw ResourceError.NotFound(ResourceTypes.Metric, id);
        }

        public async ValueTask<Series> GetAsync(string name, string granularity = "PT1M")
        {
            #region Preconditions

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            #endregion

            if (!cache.TryGetValue((name, granularity), out var series))
            {
                series = await FindAsync(name, granularity) 
                      ?? await CreateAsync(name, granularity);
                
                cache.TryAdd((name, granularity), series);
            }

            return series;
        }

        public Task<Series> FindAsync(string name, string granularity = "PT1M")
        {
             return db.Series.QueryFirstOrDefaultAsync(
                And(Eq("name", name), Eq("granularity", granularity))
            );
        }

        private async Task<Series> CreateAsync(string name, string granularity)
        {
            var series = new Series(
                id          : await db.Series.Sequence.NextAsync(),
                name        : name,
                granularity : granularity
            );

            await db.Series.InsertAsync(series);

            return series;
        }

        public async Task<IReadOnlyList<SeriesPoint>> GetDataPoints(long id, DateRange range)
        {
            long start = new Timestamp(range.Start).Value;
            long end   = new Timestamp(range.End).Value;

            return await db.SeriesPoints.QueryAsync(
                And(Eq("seriesId", id), Between("timestamp", start, end))
            );
        }

        /*
        public void ArchiveAsync(Series source, Series target, DateRange period)
        {
            // Rollup a source series
            // change from 1m Granularity to 5 minute Granularity
        }
        */
    }
}