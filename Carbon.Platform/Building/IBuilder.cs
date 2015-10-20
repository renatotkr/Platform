namespace Carbon.Platform
{
	using System.Threading.Tasks;

	public interface IBuilder
	{
		Task<BuildResult> BuildAsync();
	}
}

// BuildStyles
// BuildScripts
// BuildCode