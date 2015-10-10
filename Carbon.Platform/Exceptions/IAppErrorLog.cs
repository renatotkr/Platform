namespace Carbon.Platform
{
	public interface IAppErrorLog
	{
		bool Create(AppError error);
	}
}