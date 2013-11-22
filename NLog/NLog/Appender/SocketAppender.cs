using System.Text;

namespace NLog {
    public class SocketAppender : SocketAppenderBase {
        bool _useColorCodes;

        public SocketAppender(bool useColorCodes = false) {
            _useColorCodes = useColorCodes;
        }

        protected override byte[] SerializeMessage(string message, LogLevel logLevel) {
            if (_useColorCodes)
                message = ColorCodeFormatter.FormatMessage(message, logLevel);

            return Encoding.ASCII.GetBytes(message + "\n");
        }
    }
}