using System;
using System.Collections.Generic;

namespace NLog {
    public enum LogLevel : byte {
        On = 0,
        Trace = 1,
        Debug = 2,
        Info = 3,
        Warn = 4,
        Error = 5,
        Fatal = 6,
        Off = 7
    }

    public class Logger {
        public delegate void Appender(string message, LogLevel logLevel);

        public string name { get; private set; }

        public LogLevel minLogLevel { get; set; }

        public Appender appender { get; set; }

        static readonly Dictionary<LogLevel, string> _logLevelPrefixes = new Dictionary<LogLevel, string>() {
            { LogLevel.Trace, "[TRACE]" },
            { LogLevel.Debug, "[DEBUG]" },
            { LogLevel.Info,  "[INFO] " },
            { LogLevel.Warn,  "[WARN] " },
            { LogLevel.Error, "[ERROR]" },
            { LogLevel.Fatal, "[FATAL]" }
        };

        public Logger(string name, LogLevel minLogLevel) {
            this.name = name;
            this.minLogLevel = minLogLevel;
        }

        void log(string message, LogLevel logLevel) {
            if (logLevel >= minLogLevel && appender != null) {
                var time = String.Format("{0:hh:mm:ss:fff}", DateTime.Now);
                string logMessage = String.Format("{0} {1} {2}: {3}",
                                        _logLevelPrefixes[logLevel],
                                        time,
                                        name,
                                        message);
                appender(logMessage, logLevel);
            }
        }

        public void Trace(string message) {
            log(message, LogLevel.Trace);
        }

        public void Debug(string message) {
            log(message, LogLevel.Debug);
        }

        public void Info(string message) {
            log(message, LogLevel.Info);
        }

        public void Warn(string message) {
            log(message, LogLevel.Warn);
        }

        public void Error(string message) {
            log(message, LogLevel.Error);
        }

        public void Fatal(string message) {
            log(message, LogLevel.Fatal);
        }

        public void Assert(bool condition, string message) {
            if (!condition)
                throw new NLogAssert(message);
        }
    }

    public class NLogAssert : Exception {
        public NLogAssert(string message) : base(message) {
        }
    }
}

