namespace Carbon.Platform
{
	using System.Collections.Generic;

	public interface IApp
	{
		int Id { get; }

		string Name { get; }

		AppType Type { get; }

		IList<BindingInfo> Bindings { get; }
	}
}