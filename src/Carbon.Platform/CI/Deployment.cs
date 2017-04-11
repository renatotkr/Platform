﻿using System;

using Carbon.Data.Annotations;
using Carbon.Versioning;

namespace Carbon.Platform.CI
{
    [Dataset("Deployments")]
    public class Deployment : IDeployment
    {
        public Deployment() { }

        public Deployment(
            long id,
            long appId,
            SemanticVersion revision, 
            DeploymentStatus status = DeploymentStatus.Pending)
        {
            #region Preconditions

            if (revision == SemanticVersion.Zero)
                throw new ArgumentException("Must not be Zero", nameof(revision));

            #endregion

            Id       = id;
            Status   = status;
            AppId    = appId;
            Revision = revision.ToString();
        }

        // envId + sequence
        [Member("id"), Key]
        public long Id { get; }

        [Member("status")]
        public DeploymentStatus Status { get; set; }
       
        [Member("appId")]
        public long AppId { get; }
       
        [Member("revision")]
        public string Revision { get; }

        [Member("commitId")]
        public long CommitId { get; set; }

        [Member("creatorId")]
        public long CreatorId { get; set; }

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("completed")]
        public DateTime? Completed { get; set; }

        #endregion

        public long EnvironmentId => ScopedId.GetScope(Id);
    }
}