using System;
using Carbon.Data.Sequences;

namespace Carbon.Rds
{
    public interface IDatabaseBackup
    {
        DateTime? Completed { get; set; }
        long Id { get; }
        Uid? KeyId { get; }
        string Message { get; }
        string Name { get; }
        long Size { get; }
        DatabaseBackupStatus Status { get; }
    }
}