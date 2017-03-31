using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Carbon.Diagnostics
{
    public class DiagnosticList : Collection<Diagnostic>
    {
        public IEnumerable<Diagnostic> Messages => 
            this.Where(b => b.Type == DiagnosticType.Message);

        public IEnumerable<Diagnostic> Warnings => 
            this.Where(b => b.Type == DiagnosticType.Warning);

        public IEnumerable<Diagnostic> Errors => 
            this.Where(b => b.Type == DiagnosticType.Error);
    }
}