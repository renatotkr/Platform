using System;

namespace Carbon.Platform
{
    public interface IRepository
    {
        long Id { get; }

        Uri Url { get; }
    }
}