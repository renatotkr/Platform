using System;

namespace Carbon.Platform.Computing
{
    using Protection;

    public class ContainerImage
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string ImageId { get; set; } // sha256:77af4d6b9913e693e8d0b4b294fa62ade6054e6b2f1ffb617ac955dd63fb0182

        public string Repository { get; set; } // ubuntu, training/webapp

        public string Tag { get; set; }
        
        public DateTime Created { get; set; }

        public Hash Digest { get; set; } // sha256:90305c9112250c7e3746425477f1c4ef112b03b4abe78c612e092037bfecc3b7

        public long Size { get; set; }
    }
}

// DockerImages