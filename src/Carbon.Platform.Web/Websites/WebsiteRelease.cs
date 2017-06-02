using System;
using System.Runtime.Serialization;

using Carbon.Data.Annotations;
using Carbon.Platform.Storage;
using Carbon.Versioning;
using Carbon.CI;

namespace Carbon.Platform.Web
{
    [Dataset("WebsiteReleases")]
    [UniqueIndex("websiteId", "version")]
    public class WebsiteRelease : IRelease, IWebsite
    {
        public WebsiteRelease() { }

        public WebsiteRelease(
            long id,
            IWebsite website,
            SemanticVersion version, 
            IPackageInfo package,
            IRepositoryCommit commit,
            long creatorId)
        {
            #region Preconditions

            if (website == null)
                throw new ArgumentNullException(nameof(website));

            if (package == null)
                throw new ArgumentNullException(nameof(package));

            if (package.Sha256.Length != 32)
                throw new ArgumentException("Must be 32", nameof(package.Sha256));


            if (commit == null)
                throw new ArgumentNullException(nameof(commit));

            #endregion

            Id        = id;
            WebsiteId = website.Id;
            Version   = version;
            CommitId  = commit.Id;
            CreatorId = creatorId;
            PackageName = package.Name;
            PackageDekId = package.DekId;
            PackageIV = package.IV;
            PackageSha256 = package.Sha256;
        }

        // websiteId + sequenceNumber
        [Member("id"), Key]
        public long Id { get; }

        [Member("websiteId")]
        public long WebsiteId { get; }

        [Member("websiteName")]
        public string WebsiteName { get; }

        [Member("version")]
        public SemanticVersion Version { get; }

        [Member("commitId")] 
        public long CommitId { get; }

        [Member("creatorId")]
        public long CreatorId { get; }

        #region IRelease

        ReleaseType IRelease.Type => ReleaseType.Website;

        #endregion

        #region Package
        
        [Member("packageName")]
        [StringLength(100)]
        public string PackageName { get; set; }

        [Member("packageDekId")]
        public long? PackageDekId { get; set; }

        [Member("packageIV")]
        [FixedSize(16)]
        public byte[] PackageIV { get; set; }

        [Member("packageSha256")]
        [FixedSize(32)]
        public byte[] PackageSha256 { get; set; }

        [IgnoreDataMember]
        public IPackageInfo Package =>
            new WebsitePackage(PackageName, PackageDekId, PackageIV, PackageSha256);

        #endregion

        #region IWebsite

        string IWebsite.Name => WebsiteName;

        #endregion

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        #endregion
    }

    internal class WebsitePackage : IPackageInfo
    {
        public WebsitePackage(
            string name,
            long? dekId,
            byte[] iv,
            byte[] sha256)
        {
            Name   = name;
            DekId  = dekId;
            IV     = iv;
            Sha256 = sha256;
        }

        public string Name { get; }

        public long? DekId { get; }

        public byte[] IV { get; }

        public byte[] Sha256 { get; }
    }
}
