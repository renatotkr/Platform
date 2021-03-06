﻿namespace Carbon.Platform.Computing
{
    public class CreateHostProgramRequest
    {
        public CreateHostProgramRequest(IHost host, IProgram program)
        {
            HostId  = host.Id;
            Program = program;
        }

        public long HostId { get; }

        public IProgram Program { get; }
    }
}