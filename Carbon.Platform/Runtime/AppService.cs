namespace Carbon.Platform
{
	using System;
	using System.IO;

	public class AppService
	{
		private static AppInstance instance;

		private readonly object mutex = new object();

		public AppInstance GetCurrentInstance()
		{
			if(instance == null)
			{
				var fileName = GetInstanceFileName();

				if (!File.Exists(fileName)) return null;

				lock (mutex)
				{
					var text = File.ReadAllText(fileName);

					instance = AppInstance.FromKey(text);

					// 1a0000000a0000002a000000

					// AppTagId.Create()

				}
			}

			return instance;
		}

		#region Helpers

		private string GetInstanceFileName()
		{
			var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

			return Path.Combine(baseDirectory, "instance.txt");
		}

		#endregion
	}
}

/*
instance.txt
23000000030000002d000000
*/
/*

deploy.toml
 
appId		= 1
appVersion	= 16
machineId	= 35
deployed    = "2014-04-01"
activated   = "2014-04-01"

*/