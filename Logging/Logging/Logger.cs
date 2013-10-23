using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Logging {

    public enum LogLevel : int {
        Debug = 1,
        Info = 2,
        Warn = 3,
        Error = 4,
        Fatal = 5
    }

    public class Logger {

        public delegate void Appender(LogLevel logLevel, string message);

        private static List<Appender> _appender = new List<Appender>();
        private static List<string> _ignore = new List<string>();
        private static Dictionary<LogLevel, string> _logLevelPrefixes = new Dictionary<LogLevel, string>() {
            {LogLevel.Debug, "[DEBUG]"},
            {LogLevel.Info, "[INFO] "},
            {LogLevel.Warn, "[WARN] "},
            {LogLevel.Error, "[ERROR]"},
            {LogLevel.Fatal, "[FATAL]"}
        };

        public static void AddAppender(Appender appender) {
            _appender.Add(appender);
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
                foreach (var appender in _appender)
                    appender(logLevel, time + " " + _logLevelPrefixes[logLevel] + " " + _name + ": " + message);
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

