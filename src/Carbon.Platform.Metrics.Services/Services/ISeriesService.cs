using System.Collections.Generic;
using System.Threading.Tasks;
using Carbon.Time;

namespace Carbon.Platform.Metrics
{
    public interface ISeriesService
    {
        ValueTask<Series> GetAsync(long id);

        ValueTask<Series> GetAsync(string name, string granularity = "PT1M");

        Task<Series> FindAsync(string name, string granularity = "PT1M");

        Task<IReadOnlyList<SeriesPoint>> GetDataPoints(long id, DateRange range);
    }
}