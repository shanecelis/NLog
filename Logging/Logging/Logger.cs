using System;
using System.Collections.Generic;

namespace Logging {

    public enum LogLevel {
        Debug   = 1 << 1,
        Info    = 1 << 2,
        Warn    = 1 << 3,
        Error   = 1 << 4,
        Fatal   = 1 << 5
    }

    public class Logger {

        public delegate void Appender(LogLevel logLevel,string message);

        private static Appender _appender;
        private static List<string> _ignore = new List<string>();
        private static Dictionary<LogLevel, string> _logLevelPrefixes = new Dictionary<LogLevel, string>() {
            {LogLevel.Debug, "[DEBUG]"},
            {LogLevel.Info, "[INFO] "},
            {LogLevel.Warn, "[WARN] "},
            {LogLevel.Error, "[ERROR]"},
            {LogLevel.Fatal, "[FATAL]"}
        };

        public static void AddAppender(Appender appender) {
            if (_appender == null)
                _appender = appender;
            else
                _appender += appender;
        }

        public static void RemoveAppender(Appender appender) {
            if (_appender != null)
                _appender -= appender;
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

        private string _name;

        public Logger(string name) {
            _name = name;
        }

        private void log(LogLevel logLevel, string message) {
            if (!_ignore.Contains(_name)) {
                var time = String.Format("{0:hh/mm/ss/fff}", DateTime.Now);
                string logMessage = String.Format("{0} {1} {2}: {3}",
                                                  time,
                                                  _logLevelPrefixes[logLevel],
                                                  _name,
                                                  message);
                _appender(logLevel, logMessage);
            }
        }

        public void Debug(string message) {
            log(LogLevel.Debug, message);
        }

        public void Info(string message) {
            log(LogLevel.Info, message);
        }

        public void Warn(string message) {
            log(LogLevel.Warn, message);
        }

        public void Error(string message) {
            log(LogLevel.Error, message);
        }

        public void Fatal(string message) {
            log(LogLevel.Fatal, message);
        }
    }
}

