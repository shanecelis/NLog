using System;
using System.Collections.Generic;

namespace NLog {
    public enum LogLevel {
        Trace = 1 << 1,
        Debug = 1 << 2,
        Info  = 1 << 3,
        Warn  = 1 << 4,
        Error = 1 << 5,
        Fatal = 1 << 6
    }

    public class Logger {
        public delegate void Appender(string message, LogLevel logLevel);

        static Appender _appender;
        static List<string> _ignore = new List<string>();
        static Dictionary<LogLevel, string> _logLevelPrefixes = new Dictionary<LogLevel, string>() {
            { LogLevel.Trace, "[TRACE]" },
            { LogLevel.Debug, "[DEBUG]" },
            { LogLevel.Info,  "[INFO] " },
            { LogLevel.Warn,  "[WARN] " },
            { LogLevel.Error, "[ERROR]" },
            { LogLevel.Fatal, "[FATAL]" }
        };

        public static void AddAppender(Appender appender) {
            _appender += appender;
        }

        public static void RemoveAppender(Appender appender) {
            _appender -= appender;
        }

        public static void RemoveAllAppender() {
            _appender = null;
        }

        public static Logger GetLogger(Type type) {
            return GetLogger(type.ToString());
        }

        public static Logger GetLogger(string name) {
            return new Logger(name);
        }

        public static void Ignore(Type type) {
            Ignore(type.ToString());
        }

        public static void Ignore(string ignore) {
            _ignore.Add(ignore);
        }

        public string Name { get { return _name; } }

        string _name;

        public Logger(string name) {
            _name = name;
        }

        void log(string message, LogLevel logLevel) {
            if (_appender != null && !_ignore.Contains(_name)) {
                var time = String.Format("{0:hh:mm:ss:fff}", DateTime.Now);
                string logMessage = String.Format("{0} {1} {2}: {3}",
                                        _logLevelPrefixes[logLevel],
                                        time,
                                        _name,
                                        message);
                _appender(logMessage, logLevel);
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
    }
}

