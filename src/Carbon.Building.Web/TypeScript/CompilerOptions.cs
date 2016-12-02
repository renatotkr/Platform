﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TypeScript
{
    public enum JavaScriptVersion
    {
        ES3,
        ES5,
        ES6
    }

    // https://github.com/Microsoft/TypeScript/wiki/Compiler-Options

    public class CompilerOptions
    {
        public CompilerOptions(string projectPath)
        {
            #region Preconditions

            if (projectPath == null)
                throw new ArgumentNullException(nameof(projectPath));

            #endregion

            ProjectPath = projectPath;
        }

        public string ProjectPath { get;  }

        public bool EmitComments { get; set; }

        public bool GenerateDeclaration { get; set; }

        public bool GenerateSourceMaps { get; set; }

        public string OutPath { get; set; }

        public JavaScriptVersion? TargetVersion { get; set; }

        public override string ToString()
        {
            var d = new Dictionary<string, string>();

            // By invoking tsc with no input files and a - project(or just - p) command line option
            // that specifies the path of a directory containing a tsconfig.json file.

            d.Add("-p", ProjectPath);
 
            if (EmitComments == false)
            {
                d.Add("--removeComments", null);
            }

            if (GenerateDeclaration)
            {
                d.Add("-d", null);
            }

            if (GenerateSourceMaps)
            {
                d.Add("--sourcemap", null);
            }

            if (!string.IsNullOrEmpty(OutPath))
            {
                d.Add("--out", OutPath);
            }

            if (TargetVersion != null)
            {
                d.Add("--target", TargetVersion.Value.ToString());
            }

            return string.Join(" ", d.Select(o => o.Key + " " + o.Value));
        }
    }
}