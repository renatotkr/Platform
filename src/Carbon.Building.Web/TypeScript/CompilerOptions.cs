﻿using System;
using System.Text;

namespace TypeScript
{
    public class CompilerOptions
    {
        public CompilerOptions(string projectPath)
        {
            ProjectPath = projectPath ?? throw new ArgumentNullException(nameof(projectPath));
        }

        public string ProjectPath { get; }

        public bool EmitComments { get; set; }

        public bool GenerateDeclaration { get; set; }

        public bool GenerateSourceMaps { get; set; }

        public string OutPath { get; set; }

        public JavaScriptVersion? TargetVersion { get; set; }

        public bool AllowJs { get; set; }

        public bool AlwaysStrict { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();

            // By invoking tsc with no input files and a - project(or just - p) command line option
            // that specifies the path of a directory containing a tsconfig.json file.

            sb.WriteOption("-p", ProjectPath);

            if (AllowJs)
            {
                sb.Append(" --allowJs");
            }

            if (AlwaysStrict)
            {
                sb.Append(" --alwaysStrict");
            }

            if (EmitComments == false)
            {
                sb.Append(" --removeComments");
            }

            if (GenerateDeclaration)
            {
                sb.Append(" --declaration");
            }

            if (GenerateSourceMaps)
            {
                sb.Append(" --sourcemap");
            }

            // outDir?
            if (!string.IsNullOrEmpty(OutPath))
            {
                sb.WriteOption("--out", OutPath);
            }


            if (TargetVersion != null)
            {
                sb.WriteOption("--target", TargetVersion.Value.ToString());
            }

            return sb.ToString();
        }       
    }

    internal static class SbExtensions
    {
        public static void WriteOption(this StringBuilder sb, string name, string value)
        {
            if (sb.Length > 0)
            {
                sb.Append(' ');
            }

            sb.Append(name);
            sb.Append(' ');
            sb.Append(value);
        }
    }
}


// http://www.typescriptlang.org/docs/handbook/compiler-options.html
