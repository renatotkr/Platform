using System.Threading.Tasks;

namespace Carbon.Platform.Metrics
{
    public interface IMetricDataProcessor
    {
        Task ProcessAsync(MetricData data);

        Task ProcessAsync(MetricData[] datas);
    }
}