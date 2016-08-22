using System;

namespace Carbon.Platform
{
    using Data.Annotations;

    [Record(TableName = "Repositories")]
    public class RepositoryInfo : IRepository
    {
        [Member(1), Identity]
        public long Id { get; set; }
        
        [Member(2)] // e.g. git://github.com/user/project.git#commit-ish
        public Uri Url { get; set; }
    }
}