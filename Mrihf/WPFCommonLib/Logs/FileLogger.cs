using CommonLib.Logs;
using System;
using System.IO;

namespace WPFCommonLib.Logs
{
    public class FileLogger : ILog
    {
        public const string LogFile = "app.log";
        public const string LogFolder = "Logs";

        public FileLogger()
        {
            try
            {
                var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LogFolder);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
            catch
            {
            }
        }

        public string FilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LogFolder, LogFile);

        public override bool Equals(object obj)
        {
            var loger = obj as FileLogger;
            return loger != null && FilePath == loger.FilePath;
        }

        public void Log(string message, LogLevel logLevel, Priority priority = Priority.None)
        {
            if (logLevel == LogLevel.Debug)
            {
                return;
            }

            try
            {
                File.AppendAllText(FilePath, $"[{logLevel.ToString()}] {DateTime.Now} {message}\r"+ priority.ToString()+ "\n");
            }
            catch (UnauthorizedAccessException ex)
            {
            }
        }
    }
}