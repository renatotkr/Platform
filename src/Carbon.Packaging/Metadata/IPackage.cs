namespace Carbon.Packaging
{
    using Versioning;

    public interface IPackage
    {
        string Name { get; }

        SemanticVersion Version { get; }
    }
}


/*
NPM Description:
A package is any of:

a) a folder containing a program described by a package.json file
b) a gzipped tarball containing(a)
c) a url that resolves to(b)
d) a <name>@<version> that is published on the registry with(c)
e) a <name>@<tag> that points to(d)
f) a <name> that has a latest tag satisfying(e)
g) a git url that, when cloned, results in (a).
*/