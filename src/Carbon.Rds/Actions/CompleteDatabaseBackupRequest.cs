namespace Carbon.Rds
{
    public class CompleteDatabaseBackupRequest
    {
        public CompleteDatabaseBackupRequest(
            long backupId, 
            DatabaseBackupStatus status, 
            long size,
            byte[] sha256,
            string message = null)
        {
            BackupId = backupId;
            Status = status;
            Size = size;
            Sha256 = sha256;
            Message = message;
        }

        public long BackupId { get; }

        public DatabaseBackupStatus Status { get; }

        public long Size { get; }

        public byte[] Sha256 { get; }

        public string Message { get; }
    }
}