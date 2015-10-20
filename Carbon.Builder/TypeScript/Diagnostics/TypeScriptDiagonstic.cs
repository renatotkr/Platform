namespace TypeScript
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;

    using Carbon.Platform;

    public class TypeScriptDiagonstic : Diagnostic
    {
        public static TypeScriptDiagonstic Parse(string text)
        {
            // D:/builds/93ef00b2abe44ca8858603bef9fa0c78/5e31664ea2831739542b0607c0183b74f38dbdee78221b5ac944edc52f70a95b.d.ts(103,5)
            // : error TS2300
            // : Duplicate identifier 'jsonp'.

            var parts = text.Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries);

            var error = new TypeScriptDiagonstic();

            if (parts.Length < 2)
            {
                error.Message = text;

                return error;
            }

            try
            {
                var fileParts = parts[0].TrimEnd(')').Split('(');
                var positionParts = fileParts[1].Split(',');

                error.FileName = fileParts[0];
                error.Line = int.Parse(positionParts[0]);
                error.Column = int.Parse(positionParts[1]);
                error.Code = parts[1].Replace("error", "");

                if (parts.Length > 2)
                {
                    error.Message = parts[2];
                }
            }
            catch
            {
                throw new Exception(parts.Length + ":" + text);
            }

            return error;
        }
    }

    
}
