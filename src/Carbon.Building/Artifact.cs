namespace Carbon.Building
{
    public class Artifact
    {
        public Artifact(string path)
        {
            Path = path;
        }

        public string Path { get; }
    }
}