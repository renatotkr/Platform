using System;

namespace Carbon.Platform
{
    using Data.Annotations;

    [Record(TableName = "Repositories")]
    public class RepositoryInfo : IRepository
    {
        [Member(1), Identity]
        public long Id { get; set; }
        
        [Member(2)]
        public Uri Url { get; set; }
    }
}

/*
Type & name can be discovered from the URL

git://github.com/user/project.git#commit-ish
*/
