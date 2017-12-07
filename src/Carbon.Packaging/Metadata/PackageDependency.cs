using System;
using System.Runtime.Serialization;
using Carbon.Versioning;

namespace Carbon.Packaging
{
    [DataContract]
    public readonly struct PackageDependency
    {
        public PackageDependency(string name, string text)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = text ?? throw new ArgumentNullException(nameof(text));
        }

        [DataMember(Name = "name", Order = 1)]
        public string Name { get; }

        [DataMember(Name = "value", Order = 2)]
        public string Value { get; }

        [IgnoreDataMember]
        public bool IsFile => !char.IsDigit(Value[0]);

        [IgnoreDataMember]
        public SemanticVersionRange VersionRange => SemanticVersionRange.Parse(Value);
    }
}

// "foo" : "1.0.0 - 2.9999.9999"
// "mocha": "visionmedia/mocha#4727d357ea"
// "asd" : "http://asdf.com/asdf.tar.gz"
// "dyl" : "file:../dyl"