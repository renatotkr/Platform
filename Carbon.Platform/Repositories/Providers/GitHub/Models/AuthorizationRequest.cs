namespace GitHub
{
	using System.ComponentModel.DataAnnotations;
	using System.Runtime.Serialization;

	public class AuthorizationRequest
	{
		/// <summary>
		/// [Optional] A list of scopes that this authorization is in.
		/// </summary>
		[DataMember(Name = "scopes", EmitDefaultValue = false)]
		public string[] Scopes { get; set; }

		/// <summary>
		/// [Optional] A note to remind you what the OAuth token is for.
		/// </summary>
		[DataMember(Name = "note", EmitDefaultValue = false)]
		public string Note { get; set; }

		/// <summary>
		/// [Optional] URL to remind you what app the OAuth token is for.
		/// </summary>
		[DataMember(Name = "note_url", EmitDefaultValue = false)]
		public string NoteUrl { get; set; }

		/// <summary>
		/// [Optional]  OAuth app client key for which to create the token.
		/// </summary>
		[DataMember(Name = "client_id", EmitDefaultValue = false)]
		[StringLength(20)]
		public string ClientId { get; set; }

		// <summary>
		/// [Optional] OAuth app client secret for which to create the token.
		/// </summary>
		[DataMember(Name = "client_secret", EmitDefaultValue = false)]
		[StringLength(40)]
		public string ClientSecret { get; set; }
	}
}
