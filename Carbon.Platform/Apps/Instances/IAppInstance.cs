namespace Carbon.Platform
{
	public interface IAppInstance
	{
		int AppId		{ get; }

		int MachineId	{ get; }

		int AppVersion	{ get; }
	}
}