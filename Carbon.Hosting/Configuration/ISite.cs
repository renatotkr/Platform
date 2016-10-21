using System.Collections.Generic;

namespace Carbon.Hosting
{
    public interface ISite
    {
        IList<SiteBindingInfo> Bindings { get; }
    }
}
