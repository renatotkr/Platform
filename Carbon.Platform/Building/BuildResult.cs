namespace Carbon.Platform
{
	using System;

	public class BuildResult
	{
		public TimeSpan Elapsed { get; set; }

		public BuildStatus Status { get; set; }

		public DiagnosticList Diagnostics { get; set; }
	}
}