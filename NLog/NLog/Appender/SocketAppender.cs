using System.Text;

namespace NLog {
    public class SocketAppender : SocketAppenderBase {
        readonly bool _useColorCodes;

        public SocketAppender(bool useColorCodes) {
            _useColorCodes = useColorCodes;
        }

        protected override byte[] serializeMessage(string message, LogLevel logLevel) {
            if (_useColorCodes)
                message = ColorCodeFormatter.FormatMessage(message, logLevel);

            return Encoding.UTF8.GetBytes(message + "\n");
        }
    }
}