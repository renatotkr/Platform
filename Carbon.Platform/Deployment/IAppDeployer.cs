namespace Carbon.Platform
{
	using System.Threading.Tasks;

	public interface IAppDeployer
	{
		// Pull the specified revision from the repository
		// Build the source
		// Package the build
		// Sign the package
		// Push the package to the PackageStore (S3)
		// Create a record of the package (App Version)

		// Task<AppVersion> Package(IApp app, Revision revision);

		Task Deploy(AppVersion version);
	}

	// Five steps. 
	// Download, Build, Test, Package, Deploy, Active

	// source: "git://carbonmade/portfolio.git;tags/1.1"

	/* ----------------------------------------------------------------------------------
	
	SCOPE:		portfolio
	 
	COMMAND					Version		ENVIROMENT
	deploy		portfolio	1.1			to	beta
	activate	portfolio	2.1.1		in  beta

	Coordinator

	POST	/apps/{id}/versions			{ name: "portfolio/1.1.1-beta", signature: "12345123" }
	PUT		/machines					{ name: "S14" }												-> 201 { }
	POST	/apps						{ name: "portfolio" ] }										-> 201 { }
	POST	/apps/{id}/versions			{ name: "2.1.1", packageName: "portfolio/1.1.1-beta" }		-> 201 { }
	
	PUT	/apps/{name}/enviroments/beta/versions/2.1.1/activate	{ version: "2.1.1" }										-> 200 { }
	
	---------------------------------------------------------------------------------- */
}



/*
repository "git://git.apache.org/couchdb.git"
reference "master"

deploy "/my/deploy/dir" do
repository "git@github.com/whoami/project"
revision "abc123" # or "HEAD" or "TAG_for_1.0"
 
You may set :branch, which is the reference to the branch, tag, or any SHA1 you are deploying, for example:
set :branch, "master"
 * 
*/