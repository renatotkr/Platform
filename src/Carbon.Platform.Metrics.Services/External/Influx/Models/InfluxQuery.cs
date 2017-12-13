using System;
using System.Text;
using System.Text.Encodings.Web;

namespace Influx
{
    public class InfluxQuery
    {
        public InfluxQuery(string database, string command, bool pretty = false)
        {
            Database = database ?? throw new ArgumentNullException(nameof(database));
            Command = command ?? throw new ArgumentNullException(nameof(command));
            Pretty = pretty;
        }

        public bool Pretty { get; }

        public string Database { get;  }

        // select sum(value) from transfer where accountId = '10000' group by time(1m)

        public string Command { get;  }

        public string ToQueryString()
        {
            var sb = new StringBuilder();

            sb.Append('?');

            sb.Append("db=");
            sb.Append(UrlEncoder.Default.Encode(Database));
            
            sb.Append("&q=");
            sb.Append(UrlEncoder.Default.Encode(Command));

            if (Pretty)
            {
                sb.Append("&pretty=true");
            }

            return sb.ToString();
        }        
    }
}
