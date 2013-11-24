using System;

namespace NLog {
    public static class LoggerFactory {
        public static LogLevel globalMinLogLevel { get; set; }

        static Logger.Appender _appender;

        public static Logger GetLogger(Type type, LogLevel loglevel = LogLevel.On) {
            return GetLogger(type.ToString(), loglevel);
        }

        public static Logger GetLogger(string name, LogLevel loglevel = LogLevel.On) {
            var logger = new Logger(name, globalMinLogLevel > loglevel ? globalMinLogLevel : loglevel);
            logger.appender = _appender;
            return logger;
        }

        public static void AddAppender(Logger.Appender appender) {
            _appender += appender;
        }

        public static void RemoveAppender(Logger.Appender appender) {
            _appender -= appender;
        }

        public static void RemoveAllAppender() {
            _appender = null;
        }
    }
}

