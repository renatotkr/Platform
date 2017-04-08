namespace Carbon.Deployment
{
    /*
    public interface IPipeline
    {
        // Download
        // Build
        // Test
        // Package
        // Publish
        // Deploy 
        // Activate

        Task DeployAsyc(IApp app);
    }
    */
    
    /* ----------------------------------------------------------------------------------
	
	COMMAND					revision	ARGS
	deploy		portfolio	1.1.0   	- enviroment production
	activate	portfolio	2.1.1		- enviroment beta

	Coordinator

	POST	/apps/{id}@1.0	         { name: "portfolio/1.1.1-beta"}
	PUT		/hosts					 { name: "S14" }										    -> 201 { }
	POST	/apps 			     	 { name: "portfolio" ] }								    -> 201 { }
	POST	/apps/{id}/releases	     { name: "2.1.1", packageName: "portfolio/1.1.1-beta" }     -> 201 { }
	
	POST	/apps/1@1.0.0/activate	            									    -> 200 { }
	
	---------------------------------------------------------------------------------- */
}