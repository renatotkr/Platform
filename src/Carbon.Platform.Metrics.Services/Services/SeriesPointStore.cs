using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carbon.Time;

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

        public async Task<IReadOnlyList<DataPoint>> ListAsync(ITimeSeries series, DateRange period)
        {
            using (var connection = await db.Context.GetConnectionAsync())
            {
                var result = await connection.QueryAsync<DataPoint>(
                    @"SELECT `timestamp`, `value` 
                      FROM `SeriesPoints`
                      WHERE `seriesId` = @seriesId
                        AND `timestamp` BETWEEN @start AND @end", new
                    {
                        seriesId = series.Id,
                        start = new Timestamp(period.Start).Value,
                        end = new Timestamp(period.End).Value
                    }
                );

                return result.AsList();
            };
        }

        public async Task IncrementAsync(IReadOnlyList<SeriesPoint> points)
        {
            using (var connection = await db.Context.GetConnectionAsync())
            {
                await connection.ExecuteAsync(
                    @"INSERT INTO `SeriesPoints`(`seriesId`, `timestamp`, `value`) VALUES (@seriesId, @timestamp, @value)
                      ON DUPLICATE KEY UPDATE `value` = `value` + @value;", points
                );
            }
        }

        public async Task IncrementAsync(SeriesPoint point)
        {
            using (var connection = await db.Context.GetConnectionAsync())
            {
                await connection.ExecuteAsync(
                    @"INSERT INTO `SeriesPoints`(seriesId, timestamp, value) VALUES (@seriesId, @timestamp, @value)
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