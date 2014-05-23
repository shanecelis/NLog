using System.Collections.Generic;

namespace NLog {
    public static class LoggerFactory {
        public static LogLevel globalLogLevel {
            get { return _globalLogLevel; }
            set {
                _globalLogLevel = value;
                foreach (var logger in _loggers.Values)
                    logger.logLevel = value;
            }
        }

        static LogLevel _globalLogLevel;
        static Logger.LogDelegate _appenders;
        readonly static Dictionary<string, Logger> _loggers = new Dictionary<string, Logger>();

        public static void AddAppender(Logger.LogDelegate appender) {
            _appenders += appender;
            foreach (var logger in _loggers.Values)
                logger.OnLog += appender;
        }

        public static void RemoveAppender(Logger.LogDelegate appender) {
            _appenders -= appender;
            foreach (var logger in _loggers.Values)
                logger.OnLog -= appender;
        }

        public static Logger GetLogger(string name) {
            if (!_loggers.ContainsKey(name))
                _loggers.Add(name, createLogger(name));

            return _loggers[name];
        }

        public static void Reset() {
            _loggers.Clear();
            _appenders = null;
        }

        static Logger createLogger(string name) {
            var logger = new Logger(name);
            logger.logLevel = globalLogLevel;
            logger.OnLog += _appenders;
            return logger;
        }
    }
}

