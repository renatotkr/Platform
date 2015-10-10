namespace Carbon.Platform
{
	using System;
	using System.Threading.Tasks;

	public interface IRepositoryClient
	{
		// ListBranches
		// GetBranch

		Task<Package> DownloadAsync(Revision revision);

		Task<ICommit> GetCommitAsync(Revision revision);

		Task TagAsync(ICommit commit, string name);
	}
}