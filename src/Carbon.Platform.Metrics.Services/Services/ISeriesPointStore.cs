using System.Collections.Generic;
using System.Threading.Tasks;
using Carbon.Time;

namespace Carbon.Platform.Metrics
{
    public interface ISeriesPointStore
    {
        Task<IReadOnlyList<DataPoint>> ListAsync(ITimeSeries series, DateRange range);

        Task IncrementAsync(IReadOnlyList<SeriesPoint> points);

        Task IncrementAsync(SeriesPoint point);

        Task InsertAsync(IList<SeriesPoint> points);

        Task InsertAsync(SeriesPoint point);
    }
}