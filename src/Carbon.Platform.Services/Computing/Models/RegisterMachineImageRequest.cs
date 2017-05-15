﻿// using System.ComponentModel.DataAnnotations;

using Carbon.Platform.Resources;

namespace Carbon.Platform.Computing
{
    public class RegisterMachineImageRequest
    {
        public RegisterMachineImageRequest() { }

        public RegisterMachineImageRequest(string name, ManagedResource resource)
        {
            Name     = name;
            Resource = resource;
        }
        
        // [Required, StringLenth(3, 128)]
        public string Name { get; set; }

        public MachineImageType Type { get; set; } = MachineImageType.Machine;

        public ManagedResource Resource { get; set; }
    }    
}