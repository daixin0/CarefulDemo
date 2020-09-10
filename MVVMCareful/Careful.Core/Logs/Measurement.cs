using Careful.Core.Ioc;
using System;
using System.Diagnostics;

namespace Careful.Core.Logs
{
    /// <summary>
    /// 测量代码运行时长
    /// </summary>
    public interface IMeasurement
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// 执行测量
        /// </summary>
        /// <typeparam name="TCategory">类别</typeparam>
        /// <param name="scope">要测量的范围</param>
        /// <returns></returns>
        IDisposable Measure<TCategory>(string scope);
    }

    public sealed class Measurement : IMeasurement
    {
        public Measurement(IContainerProvider containerProvider)
        {
            Logger = containerProvider.Resolve<ILog>();
        }

        public Measurement(ILog log)
        {
            Logger = log;
        }

        public bool IsEnabled { get; set; } = true;
        public ILog Logger { get; }

        public IDisposable Measure<TCategory>(string scope)
        {
            if (!IsEnabled)
            {
                return new DoNothingDisposable();
            }

            return new StopwatchRecorder(Logger, typeof(TCategory).Name, scope);
        }
    }

    internal sealed class DoNothingDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }

    internal sealed class StopwatchRecorder : IDisposable
    {
        private static Stopwatch _stopwatch;
        private bool _isDisposed = false;

        public StopwatchRecorder(ILog logger, string category, string scope)
        {
            Logger = logger;
            Category = category;
            Scope = scope;
            _stopwatch = Stopwatch.StartNew();
        }

        private string Category { get; }
        private ILog Logger { get; }
        private string Scope { get; }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _stopwatch.Stop();

            string content = $"测量结果：[{Category}] 执行 {Scope} 耗时: {_stopwatch.ElapsedMilliseconds / 1000d} 秒";

            // 通过日志组件
            Logger?.Log(content, LogLevel.Debug);

            // 当前环境为调试环境时，也输出到控制台和调试窗口中
            OutputToDebug(content);

            _isDisposed = true;
        }

        [Conditional("DEBUG")]
        private void OutputToDebug(string content)
        {
            Console.WriteLine(content);
            Debug.WriteLine(content);
        }
    }
}