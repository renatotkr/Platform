using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Carbon.Kms
{
    public static partial class X509Certificate2Extensions
    {
        public static IEnumerable<AlternateSubject> GetAlternateSubjectNames(this X509Certificate2 cert)
        {
            var alternateSubjectNames = cert.Extensions["2.5.29.17"];

            if (alternateSubjectNames != null)
            {
                var text = alternateSubjectNames.Format(true);

                string line;
                string[] parts;

                using (var reader = new StringReader(text))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Length == 0) continue;

                        parts = line.Split('=');

                        if (parts.Length == 2)
                        {
                            yield return new AlternateSubject(parts[0], parts[1]);
                        }
                    }
                }
            }
        }
    }
}
