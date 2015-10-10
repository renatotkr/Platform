namespace Carbon.Platform
{
	using System;

	public interface IFrontend
	{
		string Name { get; }

		Uri RepositoryUrl { get; }

        // Releases
	}
}