using System;

using Carbon.Storage;

namespace Carbon.Platform.CI
{
    public class PublishingConflict : Exception
    {
        public PublishingConflict(IPackage oldLib)
            : base($"{oldLib} has already been published")
        {
            ExistingPackage = oldLib;
        }

        public IPackage ExistingPackage { get; }
    }
}
