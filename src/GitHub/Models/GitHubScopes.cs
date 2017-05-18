using System;
using System.Collections.Generic;
using System.Text;

namespace GitHub.Models
{
    public static class GitHubScopes
    {
        /// <summary>
        /// Grants read/write access to profile info only.Note that this scope includes user:email and user:follow.
        /// </summary>
        public static readonly string User = "user";

        /// <summary>
        /// Grants read access to a user's email addresses.
        /// </summary>
        public static readonly string UserEmail = "user:email";

        /// <summary>
        /// Grants access to follow or unfollow other users.
        /// </summary>
        public static readonly string UserFollow = "user:follow";

        /// <summary>
        /// Grants read/write access to code, commit statuses, collaborators, and deployment statuses for public repositories and organizations.Also required for starring public repositories.
        /// </summary>
        public static readonly string PublicRepo = "public_repo";

        /// <summary>
        /// Grants read/write access to code, commit statuses, invitations, collaborators, adding team memberships, and deployment statuses for public and private repositories and organizations.
        /// </summary>
        public static readonly string Repo = "repo";

        /// <summary>
        /// Grants access to deployment statuses for public and private repositories.This scope is only necessary to grant other users or services access to deployment statuses, without granting access to the code.
        /// </summary>
        public static readonly string RepoDeployment = "repo_deployment";

        /// <summary>
        /// Grants read/write access to public and private repository commit statuses.This scope is only necessary to grant other users or services access to private repository commit statuses without granting access to the code.
        /// </summary>
        public static readonly string RepoStatus = "repo_status";

        /// <summary>
        /// Grants access to delete adminable repositories.
        /// </summary>
        public static readonly string delete_repo = "delete_repo";

        /// <summary>
        /// Grants read access to a user's notifications. repo also provides this access.
        /// </summary>
        public static readonly string Notifications = "notifications";

        /// <summary>
        /// Grants write access to gists.
        /// </summary>
        public static readonly string Gist = "gist";

        /// <summary>
        /// Grants read and ping access to hooks in public or private repositories.
        /// </summary>
        public static readonly string ReadRepoHook = "read:repo_hook";

        /// <summary>
        /// Grants read, write, and ping access to hooks in public or private repositories.
        /// </summary>
        public static readonly string WriteRepoHook = "write:repo_hook";

        /// <summary>
        /// Grants read, write, ping, and delete access to hooks in public or private repositories.
        /// </summary>
        public static readonly string AdminRepoHook = "admin:repo_hook";

        /// <summary>
        /// Grants read, write, ping, and delete access to organization hooks.Note: OAuth tokens will only be able to perform these actions on organization hooks which were created by the OAuth application. Personal access tokens will only be able to perform these actions on organization hooks created by a user.
        /// </summary>
        public static readonly string AdminOrgHook = "admin:org_hook";

        /// <summary>
        /// Read-only access to organization, teams, and membership.
        /// </summary>
        public static readonly string ReadOrg = "read:org";

        /// <summary>
        /// Publicize and unpublicize organization membership.
        /// </summary>
        public static readonly string WriteOrg = "write:org";

        /// <summary>
        /// Fully manage organization, teams, and memberships.
        /// </summary>
        public static readonly string AdminOrg = "admin:org";

        /// <summary>
        /// List and view details for public keys.
        /// </summary>
        public static readonly string ReadPublicKey = "read:public_key";

        /// <summary>
        /// Create, list, and view details for public keys.
        /// </summary>
        public static readonly string WritePublicKey = "write:public_key";

        /// <summary>
        /// Fully manage public keys.
        /// </summary>
        public static readonly string AdminPublicKey = "admin:public_key";

        /// <summary>
        /// List and view details for GPG keys.
        /// </summary>
        public static readonly string ReadGpgKey = "read:gpg_key";

        /// <summary>
        /// Create, list, and view details for GPG keys.
        /// </summary>
        public static readonly string WriteGpgKey = "write:gpg_key";

        /// <summary>
        /// Fully manage GPG keys.
        /// </summary>
        public static readonly string AdminGpgKey = "admin:gpg_key";
    }
}
