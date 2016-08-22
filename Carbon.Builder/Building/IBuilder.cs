using System.Threading.Tasks;

namespace Carbon.Building
{
    public interface IBuilder
    {
        Task<BuildResult> BuildAsync();
    }
}

// BuildStyles
// BuildScripts
// BuildCode