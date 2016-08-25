using System;

namespace Carbon.Storage
{
    public interface IRepository
    {
        long Id { get; }

        Uri Url { get; }
    }
}