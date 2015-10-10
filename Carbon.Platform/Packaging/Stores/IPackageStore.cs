namespace Carbon.Platform
{
	using System.Threading.Tasks;

	public interface IPackageStore
	{
		Task<Package> GetAsync(string name);

		Task PutAsync(string name, Package package);
	}
}
