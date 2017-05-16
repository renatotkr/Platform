using Carbon.Json;

namespace Carbon.Platform.Computing
{
    public class CreateHostGroupRequest
    {
        public CreateHostGroupRequest() { }

        public CreateHostGroupRequest(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public IEnvironment Environment { get; set; }

        public ILocation Location { get; set; }

        public JsonObject Details { get; set; }
    }
}