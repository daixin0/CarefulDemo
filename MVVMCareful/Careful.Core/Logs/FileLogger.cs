using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Careful.Core.Logs
{
    public class FileLogger : ILog
    {
        public const string LogFile = "app.log";
        public const string LogFolder = "Logs";

        public FileLogger()
        {
            try
            {
                var dir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), LogFolder);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
            catch
            {
            }
        }

        public string FilePath => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), LogFolder, LogFile);

        public override bool Equals(object obj)
        {
            var loger = obj as FileLogger;
            return loger != null && FilePath == loger.FilePath;
        }

        public void Log(string message, LogLevel logLevel, Priority priority = Priority.None)
        {
            try
            {
                var errorMessage = new StringBuilder();
                errorMessage.AppendLine($"[{logLevel.ToString()}] {DateTime.Now} {message}\r" + priority.ToString() + "\n");
                errorMessage.AppendLine();
                File.AppendAllText(FilePath, errorMessage.ToString());
            }
            catch (UnauthorizedAccessException ex)
            {
            }
        }
        public void LogError(string errorMessage)
        {
            try
            {
                Log($"{DateTime.Now} {errorMessage}", LogLevel.Exception);
            }
            catch (UnauthorizedAccessException ex)
            {
            }
        }
    }
}
