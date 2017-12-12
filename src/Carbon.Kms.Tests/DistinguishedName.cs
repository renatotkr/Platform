namespace Carbon.Kms
{
    public class DistinguishedName
    {
        public string CommonName { get; set; }

        public string Country { get; set; }

        public string Organization { get; set; } // ON

        public string OrganizationUnit { get; set; } // OU

        public string DistinguishedNameQualifier { get; set; }

        public string Region { get; set; } // ST

        public string Locality { get; set; } // L


        // public string SerialNumber { get; set; } 
        
        // public string Title { get; set; }
         
        // public string FirstName { get; set; } // GN (Given NAme)
         
        // public string LastName { get; set; }  // SN (Surname)
         
        // public string Initials { get; set; }
         
        // public string Pseudonym { get; set; }
         
        // public string GenerationQualifier { get; set; }

        public static DistinguishedName Parse(string text)
        {
            var subject = new DistinguishedName();

            foreach (var field in text.Split(','))
            {
                var parts = field.Split('=');

                if (parts.Length != 2)
                {
                    throw new System.Exception(field);
                }
                var key = parts[0].Trim();
                var value = parts[1].Trim();

                switch (key)
                {
                    case "C"  : subject.Country = value;          break;
                    case "ST" : subject.Region = value;           break;
                    case "L"  : subject.Locality = value;         break;
                    case "O"  : subject.Organization = value;     break;
                    case "OU" : subject.OrganizationUnit = value; break;
                    case "CN" : subject.CommonName = value;       break;
                }
            }

            return subject;
        }
    }

    // https://msdn.microsoft.com/en-us/library/aa366101(v=vs.85).aspx

    /*
    public class DistinguishedNameParser : IDisposable
    {
        private readonly StringReader reader;

        const char eofChar = '\0';

        private char current;
        
        public SubjectParser(string text)
        {
            reader = new StringReader(text)
        }

        public (string, string) ReadNext()
        {

        }

        public void ReadName()
        {
            var sb = new StringBuilder();

            Char c;

            while ((c = Next()) != '=')
            {
                sb.Append(c);
            }

            reader.Read(); // read =

            return sb;
        }

        public void ReadValue()
        {
        }

        private char Next()
        {
            if (current == eofChar)
            {
                throw new EndOfStreamException("Cannot read past EOF");
            }

            var value = reader.Read();

            if (value == -1)
            {
                current = '\0';
            }
            else
            {
                current = (char)value;
            }

            return current;
        }

        public void Dispose()
        {
            reader.Dispose();
        }
    }
    */
}

// web.com            | Domain
// charlotte@web.com  | User
// ip                 | 192.168.1.1

// Examples: 
// C=US, ST=California, L=San Francisco, O=Wikimedia Foundation, Inc., CN=*.wikipedia.org
// CN=web.com
// C=US, ST=Maryland, L=Pasadena, O=Brent Baccala, OU=FreeSoft, CN=www.freesoft.org/emailAddress=baccala @freesoft.org

// Alternative DNS Names
// DNS:magpie, DNS:magpie.example.com, DNS:puppet, DNS:puppet.example.com

// Subject Fields
// - CN : Common name
// - DN : Distingushed name
// - O  : Organization
// - C  : Country
// - ST : State
// - L  : Locality

// Alternate subject names