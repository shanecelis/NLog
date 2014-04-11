using NSpec;
using NLog;

class describe_LoggerFactory : nspec {
    void context_LoggerFactory() {
        before = () => {
            LoggerFactory.Reset();
            LoggerFactory.globalLogLevel = LogLevel.On;
        };

        it["creates a new logger"] = () => {
            var logger = LoggerFactory.GetLogger("MyLogger");
            logger.should_not_be_null();
            logger.GetType().should_be_same(typeof(Logger));
            logger.name.should_be("MyLogger");
            logger.logLevel.should_be(LogLevel.On);
        };

        it["returns same logger when name is equal"] = () => {
            var logger1 = LoggerFactory.GetLogger("MyLogger");
            var logger2 = LoggerFactory.GetLogger("MyLogger");
            logger1.should_be_same(logger2);
        };

        it["clears created loggers"] = () => {
            var logger1 = LoggerFactory.GetLogger("MyLogger");
            LoggerFactory.Reset();
            var logger2 = LoggerFactory.GetLogger("MyLogger");
            logger1.should_not_be_same(logger2);
        };

        it["creates a new logger with globalLogLevel"] = () => {
            LoggerFactory.globalLogLevel = LogLevel.Error;
            var logger = LoggerFactory.GetLogger("MyLogger");
            logger.logLevel.should_be(LogLevel.Error);
        };

        it["sets global logLevel on created logger"] = () => {
            var logger = LoggerFactory.GetLogger("MyLogger");
            logger.logLevel.should_be(LogLevel.On);
            LoggerFactory.globalLogLevel = LogLevel.Error;
            logger.logLevel.should_be(LogLevel.Error);
        };

        it["creates new logger with global appender"] = () => {
            var appenderLogLevel = LogLevel.Off;
            var appenderMessage = string.Empty;
            LoggerFactory.appenders += (log, logLevel, message) => {
                appenderLogLevel = logLevel;
                appenderMessage = message;
            };

            var appenderLogLevel2 = LogLevel.Off;
            var appenderMessage2 = string.Empty;
            LoggerFactory.appenders += (log, logLevel, message) => {
                appenderLogLevel2 = logLevel;
                appenderMessage2 = message;
            };

            var logger = LoggerFactory.GetLogger("MyLogger");
            logger.Info("hi");

            appenderLogLevel.should_be(LogLevel.Info);
            appenderMessage.should_be("hi");
            appenderLogLevel2.should_be(LogLevel.Info);
            appenderMessage2.should_be("hi");
        };

        it["sets appender on created logger"] = () => {
            var logger = LoggerFactory.GetLogger("MyLogger");
            var didLog = false;
            LoggerFactory.appenders += (log, logLevel, message) => {
                didLog = true;
            };
            logger.Info("hi");
            didLog.should_be_true();
        };

        it["clears global appenders"] = () => {
            var appenderLogLevel = LogLevel.Off;
            var appenderMessage = string.Empty;
            LoggerFactory.appenders += (log, logLevel, message) => {
                appenderLogLevel = logLevel;
                appenderMessage = message;
            };
            LoggerFactory.Reset();
            var logger = LoggerFactory.GetLogger("MyLogger");
            logger.Info("hi");
            appenderLogLevel.should_be(LogLevel.Off);
            appenderMessage.should_be(string.Empty);
        };
    }
}

