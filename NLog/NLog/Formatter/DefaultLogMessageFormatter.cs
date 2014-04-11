namespace NLog {
    public class DefaultLogMessageFormatter {
        const string Format = "[{0}] {1}: {2}";

        public string FormatMessage(Logger logger, LogLevel logLevel, string message) {
            return string.Format(Format, logLevel.ToString().ToUpper(), logger.name, message);
        }
    }
}

