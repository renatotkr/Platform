using System.Runtime.Serialization;

namespace Carbon.Platform.Computing
{
    public class LocationDetails
    {
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public int Id { get; set; }

        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }
    }
}

// aws:us-east-1
// aws:us-east-1a