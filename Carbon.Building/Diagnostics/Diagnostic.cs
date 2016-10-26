namespace Carbon.Diagnostics
{
    public class Diagnostic
    {
        public DiagnosticType Type { get; set; }

        #region Location

        public string FileName { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }

        #endregion

        public string Code { get; set; }

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