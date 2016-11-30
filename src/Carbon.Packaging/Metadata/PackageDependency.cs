using System;

namespace Carbon.Packaging
{
    using Versioning;

    public class PackageDependency
    {
        public PackageDependency(string name, string text)
        {
            #region Preconditions

            if (name == null) throw new ArgumentNullException(nameof(name));
            if (text == null) throw new ArgumentNullException(nameof(text));

            #endregion

            Name = name;
            Value = text;
        }

        public string Name { get; }

        public string Value { get; }

        public bool IsFile => !char.IsDigit(Value[0]);

        public SemanticVersionRange VersionRange => SemanticVersionRange.Parse(Value);
    }
}

// "foo" : "1.0.0 - 2.9999.9999"
// "mocha": "visionmedia/mocha#4727d357ea"
// "asd" : "http://asdf.com/asdf.tar.gz"
// "dyl" : "file:../dyl"