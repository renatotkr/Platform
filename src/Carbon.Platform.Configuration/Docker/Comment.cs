using System;

namespace Carbon.Platform.Configuration.Docker
{
    public readonly struct Comment
    {
        public Comment(string text)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public string Text { get; }
    }
}