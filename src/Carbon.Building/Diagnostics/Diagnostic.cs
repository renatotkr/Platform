using System.Runtime.Serialization;

namespace Carbon.Diagnostics
{
    [DataContract]
    public class Diagnostic
    {
        [DataMember(Name = "type", Order = 1)]
        public DiagnosticType Type { get; set; }

        [DataMember(Name = "code", Order = 2)]
        public string Code { get; set; }

        [DataMember(Name = "message", Order = 3)]
        public string Message { get; set; }

        [DataMember(Name = "fileName", Order = 4)]
        public string FileName { get; set; }

        [DataMember(Name = "line", Order = 5)]
        public int Line { get; set; }

        [DataMember(Name = "column", Order = 6)]
        public int Column { get; set; }
    }

    public enum DiagnosticType
    {
        Hidden  = 1,
        Message = 2,
        Warning = 3,
        Error   = 4
    }
}