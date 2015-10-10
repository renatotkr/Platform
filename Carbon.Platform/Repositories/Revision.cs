namespace Carbon.Platform
{
	using System;

	// AKA (ref)

	public class Revision
	{
		public static readonly Revision Master = new Revision("master", RefType.Head);

		private readonly string name;
		private readonly RefType type;

		public Revision(string name, RefType type)
		{
			#region Preconditions

			if (name == null) throw new ArgumentNullException("name");

			#endregion

			this.name = name;
			this.type = type;
		}

		public string Name => name;

		public RefType Type => type;

		public string Path
		{
			get
			{
				switch (Type)
				{
					case RefType.Commit	: return Name;
					case RefType.Tag	: return "tags/" + Name;
					case RefType.Head	: return "heads/" + Name;
					default			    : throw new Exception("Unexpected refType");
				}
			}
		}

		public override string ToString() => Path;

		public static Revision Parse(string text)
		{
			#region Preconditions

			if (text == null) throw new ArgumentNullException("text");

			#endregion

			var type = RefType.Head;
			var name = "";

			var parts = text.Split('/');

			if (parts.Length == 1)
			{
				name = parts[0];

				// revision (40 byte, 20 character hexidecimal, SHA1 or a name that denotes a particular object)
				// dae86e1950b1277e545cee180551750029cfe735

				if (name.Length == 20) 
				{
					// It's a commit ref
					return new Revision(name, RefType.Commit);
				}

				// Otherwise, it's a symbolic ref name to a specific revision
			}
			else if(parts.Length == 2)
			{
				name = parts[1];

				switch (parts[0])
				{
					case "tags":		type = RefType.Tag;		break;
					case "branches":	type = RefType.Head;	break;
					case "heads":		type = RefType.Head;	break;

					default:			throw new Exception("Unexpected type");
				}
			}

			return new Revision(name, type);
		}
	}

	public enum RefType : byte
	{
		Commit = 1,
		Head = 2,
		Tag = 3
	}


	// Deployments are a request for a specific ref(branch,sha,tag) to be deployed.
}

/*
 tags		reside in refs/tags/ namespace
 branches	reside in refs/heads/ namespace

 master - development version, soon to be 1.1
 heads/1.0 - 1.0.0 release version
 tags/1.0.0 - 1.0.0 release version
 
 A head is simply a reference to a commit object. 
 Each head has a name (branch name or tag name, etc). 
 By default, there is a head in every repository called master. 
 A repository can contain any number of heads.
 At any given time, one head is selected as the “current head.”
 This head is aliased to HEAD, always in capitals".

 Note this difference: a “head” (lowercase) refers to any one of the named heads in the repository;
 “HEAD” (uppercase) refers exclusively to the currently active head.
 This distinction is used frequently in Git documentation.

 In contrast to a head, a tag is not updated by the commit command.

 A symbolic ref name. E.g. master typically means the commit object referenced by refs/heads/master. 
*/
