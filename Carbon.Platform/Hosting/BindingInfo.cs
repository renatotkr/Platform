namespace Carbon.Platform
{
	using System;

	public class BindingInfo
	{
		public BindingInfo(int port, string hostName = null)
		{
			#region Preconditions

			if (port < 80) throw new ArgumentNullException(paramName: nameof(port), message: "Must be greater than 80");

			#endregion

			HostName = hostName;
            Port = port;
		}

		public string Protocol => "http";

		public string HostName { get; }

		public int Port { get; }

		public string IpAddress { get; set; }

		public static BindingInfo Parse(string text)
		{
			#region Preconditions

			if (text == null) throw new ArgumentNullException(nameof(text));

			#endregion

			var parts = text.Split(':');

			if (parts.Length < 3) throw new Exception("must be at least 3 parts");

			var ip = parts[0];
			var port = int.Parse(parts[1]);
			var host = parts[2];

			return new BindingInfo(port, host) { IpAddress = ip };
		}

		public string GetInformation()
		{
			return string.Format("{0}:{1}:{2}", IpAddress ?? "*", Port.ToString(), HostName ?? "");
		}

		// 192.169.4.2:80:hostName
		public override string ToString() => GetInformation();
	}
}