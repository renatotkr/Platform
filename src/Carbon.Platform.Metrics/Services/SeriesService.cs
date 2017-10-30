using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carbon.Data.Expressions;
using Carbon.Platform.Resources;
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

        public async Task<Series> GetAsync(long id)
        {
            return await db.Series.FindAsync(id) 
                ?? throw ResourceError.NotFound(ResourceTypes.Metric, id);
        }

        public async Task<Series> GetAsync(string name, string granularity = "PT1M")
        {
            if (!cache.TryGetValue((name, granularity), out var series))
            {
                series = await db.Series.QueryFirstOrDefaultAsync(
                    And(Eq("name", name), Eq("granularity", granularity))
                );

                if (series == null)
                {
                    series = new Series(
                        id          : await db.Series.Sequence.NextAsync(),
                        name        : name,
                        granularity : granularity
                    );

                    await db.Series.InsertAsync(series);
                }

                cache.TryAdd((name, granularity), series);
            }

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