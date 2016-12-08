using System.Runtime.Serialization;

namespace Carbon.Diagnostics
{
    public class Diagnostic
    {
        [DataMember(Name = "type")]
        public DiagnosticType Type { get; set; }

        #region Location

        public string FileName { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }

        #endregion

        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }
    }

    public enum DiagnosticType
    {
        Hidden = 0,
        Info = 1,
        Warning = 2,
        Error = 3
    }
}