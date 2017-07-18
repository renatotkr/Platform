using System.Runtime.Serialization;

namespace Carbon.Platform.Computing
{
    public class LocationDetails
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }

}