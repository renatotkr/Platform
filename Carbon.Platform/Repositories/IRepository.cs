namespace Carbon.Platform
{
	using System;

	public interface IRepository
	{
		string Name { get; }

		Uri Url { get; }
	}
}