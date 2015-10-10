namespace Carbon.Platform
{
	using System;

	public interface IBuild
	{
		Guid Id { get; }

		BuildStatus Status { get; }

		DateTime? Started { get; }

		DateTime? Completed { get; }
	}
}
