using System.Threading.Tasks;

namespace Carbon.Platform.Metrics
{
    public interface ISeriesService
    {
        ValueTask<Series> GetAsync(long id);

        ValueTask<Series> GetAsync(string name, string granularity = "PT1M");

        Task<Series> FindAsync(string name, string granularity = "PT1M");
    }
}