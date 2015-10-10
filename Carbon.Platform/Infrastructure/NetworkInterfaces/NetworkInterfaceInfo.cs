namespace Carbon.Platform
{
	using System.Collections.Generic;
	using System.Net;
	using System.Runtime.Serialization;

	public class NetworkInterfaceInfo
	{
		/// <summary>
		/// e.g. Intel(R) PRO/1000 MT Network Connection
		/// </summary>
		[DataMember(Name = "description")]
		public string Description { get; set; }

		[IgnoreDataMember]
		public string InstanceName { get; set; }

		[DataMember(Name = "macAddress")]
		public string MacAddress { get; set; }

		[DataMember(Name = "ipAddresses")]
		public IList<IPAddress> IpAddresses { get; set; }

		public NetworkInterfaceObserver GetObserver()
		{
			return new NetworkInterfaceObserver(this);
		}
	}
}