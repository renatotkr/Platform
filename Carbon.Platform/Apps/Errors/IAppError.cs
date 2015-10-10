namespace Carbon.Platform
{
	using System;

	public interface IAppError : IError
	{
		int AppId { get; }				// 1

		long Id { get; }				// Date based

		int? AppVersion { get; }		// 2

		int? MachineId { get; }

		string Type { get; }

		string StackTrace { get; }

		// HTTP Requests

		// Url
		// Method
	}
}