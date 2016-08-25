using System;

namespace Carbon.Storage
{
    using Data.Annotations;

    [Record(TableName = "Repositories")]
    public class RepositoryInfo : IRepository
    {
        public RepositoryInfo() { }

        public RepositoryInfo(RepositoryType type, Uri url)
        {
            Type = type;
            Url = url;
        }

        [Member(1), Identity]
        public long Id { get; set; }
        
        [Member(2)]
        public RepositoryType Type { get; }

        [Member(3), Indexed] // e.g. git://github.com/user/project.git#commit-ish
        public Uri Url { get; set; }
    }
}