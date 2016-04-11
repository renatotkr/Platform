using System.Threading.Tasks;

namespace Carbon.Platform
{
    public interface IBuilder
    {
        Task<BuildResult> BuildAsync();
    }
}

// BuildStyles
// BuildScripts
// BuildCode