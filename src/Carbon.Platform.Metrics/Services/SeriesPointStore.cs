using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapper;

namespace Carbon.Platform.Metrics
{
    public class SeriesPointStore : ISeriesPointStore
    {
        private readonly MetricsDb db;

        public SeriesPointStore(MetricsDb db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task IncrementAsync(IReadOnlyList<SeriesPoint> points)
        {
            using (var connection = await db.Context.GetConnectionAsync())
            {
                await connection.ExecuteAsync(
                    @"INSERT INTO SeriesPoints(seriesId, timestamp, value) VALUES(@seriesId, @timestamp, @value)
                      ON DUPLICATE KEY UPDATE value = value + @value;", points
                );
            }
        }

        public async Task IncrementAsync(SeriesPoint point)
        {
            using (var connection = await db.Context.GetConnectionAsync())
            {
                await connection.ExecuteAsync(
                    @"INSERT INTO SeriesPoints(seriesId, timestamp, value) VALUES(@seriesId, @timestamp, @value)
                      ON DUPLICATE KEY UPDATE value = value + @value;", point
                );
            }
        }

        public async Task InsertAsync(SeriesPoint point)
        {
            await db.SeriesPoints.InsertAsync(point);
        }

        public async Task InsertAsync(IList<SeriesPoint> points)
        {
            await db.SeriesPoints.InsertAsync(points);
        }
    }
}