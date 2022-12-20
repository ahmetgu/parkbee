namespace ApI.Data
{
    public class SessionResult
    {
        public StatusType Status { get; set; }
        public string Message { get; set; } 
        public object Value { get; set; }
    }

    public enum StatusType
    {
        Conflict,
        NotFound,
        DependencyFail,
        Error,
        Success
    }
}
