using Carbon.Platform.Resources;

namespace Carbon.Platform.CI
{
    public class CreateBuildRequest
    {
        public long RepositoryId { get; set; }

        // e.g. 6dcb09b5b57875f334f61aebed695e2e4193db5e
        public string Revision { get; set; }

        public long CreatorId { get; set; }

        public ManagedResource Resource { get; set; }
    }
}