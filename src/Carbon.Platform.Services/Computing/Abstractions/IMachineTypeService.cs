using System.Threading.Tasks;

namespace Carbon.Platform.Computing
{
    public interface IMachineTypeService
    {
        ValueTask<IMachineType> GetAsync(long id);
    }
}