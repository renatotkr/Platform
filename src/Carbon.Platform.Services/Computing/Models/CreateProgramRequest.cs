﻿using System;
using System.ComponentModel.DataAnnotations;

using Carbon.Versioning;

namespace Carbon.Platform.Computing
{
    public class CreateProgramRequest
    {
        public CreateProgramRequest() { }

        public CreateProgramRequest(
            string name, 
            long ownerId, 
            string runtime,
            string[] addresses,
            ProgramType type = ProgramType.App,
            long? parentId = null)
        {
            #region Preconditions

            Validate.Id(ownerId, nameof(ownerId));

            if (name == null || string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            #endregion

            Name      = name;
            OwnerId   = ownerId;
            Addresses = addresses;
            Runtime   = runtime;
            Type      = type;
            ParentId  = parentId;
        }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public SemanticVersion Version { get; set; } = SemanticVersion.Zero;

        public string Runtime { get; set; }

        public string[] Addresses { get; set; }

        public ProgramType Type { get; set; }

        // ConfigurationTemplate

        [StringLength(63)]
        public string Slug { get; set; }
        
        public long? ParentId { get; set; }
        
        [Range(1, 2_199_023_255_552)]
        public long OwnerId { get; set; }
    }
}