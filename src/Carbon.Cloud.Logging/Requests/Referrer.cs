using System;
using System.Security.Cryptography;
using System.Text;
using Carbon.Data.Annotations;

namespace Carbon.Cloud.Logging
{
    [Dataset("Referrers", Schema = "Logging")]
    public class Referrer
    {
        public Referrer() { }

        public Referrer(Uri uri)
        {
            #region Preconditions

            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            #endregion

            Hash   = ComputeHash(uri);
            Scheme = uri.Scheme;
            Host   = uri.Host;
            Path   = uri.PathAndQuery;
        }
        
        [Member("id"), Key("referrerId")] 
        public long Id { get; set; }

        [Member("hash"), FixedSize(20), Unique] // sha1(uri)
        public byte[] Hash { get; }

        [Member("scheme")]
        [StringLength(10)]
        public string Scheme { get; }

        [Member("host")]
        [StringLength(1, 255)]
        public string Host { get; }

        [Member("path")]
        [StringLength(2048)]
        public string Path { get; }

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        public static byte[] ComputeHash(Uri uri)
        {
            var bytes = Encoding.UTF8.GetBytes(uri.ToString());
          
            using (var sha1 = SHA1.Create())
            {
                return sha1.ComputeHash(bytes);
            }
        }
    }
}