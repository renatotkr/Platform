using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Carbon.Diagnostics
{
    public class DiagnosticList : Collection<Diagnostic>
    {
        public IEnumerable<Diagnostic> Information
            => this.Where(b => b.Type == DiagnosticType.Info);

        public IEnumerable<Diagnostic> Warnings
            => this.Where(b => b.Type == DiagnosticType.Warning);

        public IEnumerable<Diagnostic> Errors
            => this.Where(b => b.Type == DiagnosticType.Error);
    }
}
