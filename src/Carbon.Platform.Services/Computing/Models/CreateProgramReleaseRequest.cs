﻿using System;
using System.ComponentModel.DataAnnotations;

using Carbon.Json;
using Carbon.Versioning;

namespace Carbon.Platform.Computing
{
    public class RegisterProgramReleaseRequest
    {
        public RegisterProgramReleaseRequest(
            IProgram program, 
            SemanticVersion version,
            long commitId,
            long creatorId,
            long? buildId = null,
            JsonObject properties = null)
        {
            #region Preconditions

            Validate.Id(creatorId, nameof(creatorId));

            #endregion

            Program    = program ?? throw new ArgumentNullException(nameof(program));
            Version    = version;
            CreatorId  = creatorId;
            CommitId   = commitId;
            Properties = properties ?? new JsonObject();
        }

        [Required]
        public IProgram Program { get; }

        public SemanticVersion Version { get; }

        public long CommitId { get; }

        public long? BuildId { get; }

        [Range(1, 2_199_023_255_552)]
        public long CreatorId { get; }

        public JsonObject Properties { get; }
    }
}