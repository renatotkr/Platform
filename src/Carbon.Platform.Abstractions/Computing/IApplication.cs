namespace Carbon.Platform.Computing
{
    public interface IApplication : IProgram
    {
        string[] Urls { get; }
    }
}

// *:8000         
// localhost
// http://localhost:60000
// https://*:5004

// "server.urls": "http://localhost:60000;http://localhost:60001"

// scheme://host:port