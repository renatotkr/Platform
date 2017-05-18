using System;
using System.Runtime.Serialization;

namespace GitHub
{
    public class GitAgent
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "date")]
        public DateTime? Date { get; set; }
    }
}

/*
{
  "name": "Monalisa Octocat",
  "email": "support@github.com",
  "date": "2011-04-14T16:00:49Z"
 }
*/
