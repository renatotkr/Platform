namespace Carbon.Platform
{
	using System;
	using System.Threading.Tasks;

	public interface IRepositoryClient
	{
		// ListBranches
		// GetBranch

		Task<Package> Download(Revision revision);

		Task<ICommit> GetCommit(Revision revision);

		Task Tag(ICommit commit, string name);
	}
}