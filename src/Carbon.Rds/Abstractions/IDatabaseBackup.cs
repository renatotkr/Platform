using System;

using Carbon.Data.Sequences;

namespace Carbon.Rds
{
    public interface IDatabaseBackup
    {
        long Id { get; }

        string Name { get; }

        long Size { get; }

        string Message { get; }

        Uid? KeyId { get; } // EncryptionKeyId?

        DatabaseBackupStatus Status { get; }

        DateTime? Completed { get; set; }
    }
}