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

        public static Logger.LogDelegate appenders;
        static LogLevel _globalLogLevel;
        readonly static Dictionary<string, Logger> _loggers = new Dictionary<string, Logger>();

        public static Logger GetLogger(string name) {
            if (!_loggers.ContainsKey(name))
                _loggers.Add(name, createLogger(name));

            return _loggers[name];
        }

        public static void Reset() {
            _loggers.Clear();
            appenders = null;
        }

        static Logger createLogger(string name) {
            var logger = new Logger(name);
            logger.logLevel = globalLogLevel;
            logger.OnLog += appenders;
            return logger;
        }
    }
}

