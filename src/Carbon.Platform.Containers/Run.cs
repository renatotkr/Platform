using System.Text;

namespace Carbon.Docker
{
    public class Run
    {
        public RunMode Mode { get; set; }

        public bool Cleanup { get; set; }

        public RestartPolicy RestartPolicy { get; set; }
        /*
        -a=[]           : Attach to `STDIN`, `STDOUT` and/or `STDERR`
        -t=false        : Allocate a pseudo-tty
        --sig-proxy=true: Proxify all received signal to the process (non-TTY mode only)
        -i=false        : Keep STDIN open even if not attached

        OR attach specific streams: -a stdin -a stdout -i -t ubuntu /bin/bash
        attaches all streams by default
       */

        // public string Attach { get; set; }

        public string ImageName { get; set; } // 

        // By default, all containers have the PID namespace enabled.
        // If overriden, shared with host...
        public bool UseHostProcessIdNamespace { get; set; }

        public string ContainerIdFile { get; set; } // --cidfile=""

        public long? MemoryLimit { get; set; }

        public int? BlockIOWeight { get; set; }

        public int MemorySwappiness { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (Mode == RunMode.Background)
            {
                // When running in 'detached mode', IO is no longer forwarded to the command prompt

                sb.Append(" -d"); // Detached
            }

            // $ docker run --pid=host rhel7 strace -p 1234

            if (UseHostProcessIdNamespace)
            {
                // --pid=host
                sb.Append(" --pid=host");
            }

            if (Cleanup)
            {
                sb.Append(" --rm");
            }


            return sb.ToString();
        }
    }
}