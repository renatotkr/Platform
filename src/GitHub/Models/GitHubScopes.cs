namespace GitHub.Models
{
    public static class GitHubScopes
    {
        /// <summary>
        /// Grants read/write access to profile info only.Note that this scope includes user:email and user:follow.
        /// </summary>
        public const string User = "user";

        /// <summary>
        /// Grants read access to a user's email addresses.
        /// </summary>
        public const string UserEmail = "user:email";

        /// <summary>
        /// Grants access to follow or unfollow other users.
        /// </summary>
        public const string UserFollow = "user:follow";

        /// <summary>
        /// Grants read/write access to code, commit statuses, collaborators, and deployment statuses for public repositories and organizations.Also required for starring public repositories.
        /// </summary>
        public const string PublicRepo = "public_repo";

        /// <summary>
        /// Grants read/write access to code, commit statuses, invitations, collaborators, adding team memberships, and deployment statuses for public and private repositories and organizations.
        /// </summary>
        public const string Repo = "repo";

        /// <summary>
        /// Grants access to deployment statuses for public and private repositories.This scope is only necessary to grant other users or services access to deployment statuses, without granting access to the code.
        /// </summary>
        public const string RepoDeployment = "repo_deployment";

        /// <summary>
        /// Grants read/write access to public and private repository commit statuses.This scope is only necessary to grant other users or services access to private repository commit statuses without granting access to the code.
        /// </summary>
        public const string RepoStatus = "repo_status";

        /// <summary>
        /// Grants access to delete adminable repositories.
        /// </summary>
        public const string delete_repo = "delete_repo";

        /// <summary>
        /// Grants read access to a user's notifications. repo also provides this access.
        /// </summary>
        public const string Notifications = "notifications";

        /// <summary>
        /// Grants write access to gists.
        /// </summary>
        public const string Gist = "gist";

        /// <summary>
        /// Grants read and ping access to hooks in public or private repositories.
        /// </summary>
        public const string ReadRepoHook = "read:repo_hook";

        /// <summary>
        /// Grants read, write, and ping access to hooks in public or private repositories.
        /// </summary>
        public const string WriteRepoHook = "write:repo_hook";

        /// <summary>
        /// Grants read, write, ping, and delete access to hooks in public or private repositories.
        /// </summary>
        public const string AdminRepoHook = "admin:repo_hook";

        /// <summary>
        /// Grants read, write, ping, and delete access to organization hooks.Note: OAuth tokens will only be able to perform these actions on organization hooks which were created by the OAuth application. Personal access tokens will only be able to perform these actions on organization hooks created by a user.
        /// </summary>
        public const string AdminOrgHook = "admin:org_hook";

        /// <summary>
        /// Read-only access to organization, teams, and membership.
        /// </summary>
        public const string ReadOrg = "read:org";

        /// <summary>
        /// Publicize and unpublicize organization membership.
        /// </summary>
        public const string WriteOrg = "write:org";

        /// <summary>
        /// Fully manage organization, teams, and memberships.
        /// </summary>
        public const string AdminOrg = "admin:org";

        /// <summary>
        /// List and view details for public keys.
        /// </summary>
        public const string ReadPublicKey = "read:public_key";

        /// <summary>
        /// Create, list, and view details for public keys.
        /// </summary>
        public const string WritePublicKey = "write:public_key";

        /// <summary>
        /// Fully manage public keys.
        /// </summary>
        public const string AdminPublicKey = "admin:public_key";

        /// <summary>
        /// List and view details for GPG keys.
        /// </summary>
        public const string ReadGpgKey = "read:gpg_key";

        /// <summary>
        /// Create, list, and view details for GPG keys.
        /// </summary>
        public const string WriteGpgKey = "write:gpg_key";

        /// <summary>
        /// Fully manage GPG keys.
        /// </summary>
        public const string AdminGpgKey = "admin:gpg_key";
    }
}
