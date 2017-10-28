using System.Threading.Tasks;

namespace Carbon.Platform.Metrics
{
    public interface IMetricService
    {
        Task<Metric> CreateAsync(CreateMetricRequest request);

        Task<Metric> GetAsync(long id);

        Task<Metric> GetAsync(string name);
    }
}