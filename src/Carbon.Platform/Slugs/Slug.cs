using System;

using Carbon.Data.Annotations;

namespace Carbon.Platform
{
    [Dataset("Slugs")]
    public class Slug
    {
        public Slug() { }

        public Slug(
            long id,
            string name,
            long? ownerId = null, 
            SlugFlags flags = SlugFlags.None)
        {
            Id      = id;
            Name    = name ?? throw new ArgumentNullException(nameof(name));
            OwnerId = ownerId;
            Flags   = flags;
        }

        [Member("id"), Key(sequenceName: "slugId")]
        public long Id { get; }

        [Member("name"), Unique]
        [StringLength(63)]
        public string Name { get; }
        
        [Member("flags")]
        public SlugFlags Flags { get; }

        // May be claimed when null
        [Member("ownerId")]
        public long? OwnerId { get; }

        #region Timestamps

        [Member("created"), Timestamp]
        public DateTime Created { get; }

        [Member("modified")]
        public DateTime Modified { get; }

        #endregion
    }

    [Flags]
    public enum SlugFlags
    {
        None        = 0,
        Reserved    = 1 << 0,
        Trademarked = 1 << 1,
        Obscene     = 1 << 2,
    }
}


// Slugs are a globally unique identifier
// Reserving a slug allows it's use across each resource type
// Effectively a namespace

// TODO: Move to graph API w/ entities

// slug rules: 
// - lowercase
// - hostname label (< 63 characters)