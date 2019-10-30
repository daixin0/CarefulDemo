namespace CommonLib.Logs
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warn,
        Exception
    }

    public interface ILog
    {
        void Log(string message, LogLevel logLevel, Priority priority = Priority.None);
        
    }
}