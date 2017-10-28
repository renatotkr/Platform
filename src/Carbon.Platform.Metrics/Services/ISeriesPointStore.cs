using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carbon.Platform.Metrics
{
    public interface ISeriesPointStore
    {
        Task IncrementAsync(IReadOnlyList<SeriesPoint> points);

        Task IncrementAsync(SeriesPoint point);

        Task InsertAsync(IList<SeriesPoint> points);

        Task InsertAsync(SeriesPoint point);
    }
}