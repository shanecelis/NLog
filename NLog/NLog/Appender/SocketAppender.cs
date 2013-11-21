namespace NLog {
    public class SocketAppender : SocketAppenderBase {
        bool _useColorCodes;

        public SocketAppender(bool useColorCodes = false) {
            _useColorCodes = useColorCodes;
        }

        protected override byte[] SerializeMessage(string message, LogLevel logLevel) {
            if (_useColorCodes)
                message = ColorCodeFormatter.FormatMessage(message, logLevel);

            message += "\n";
            char[] msgString = message.ToCharArray();
            byte[] byteArray = new byte[message.Length];
            for (int i = 0; i < msgString.Length; i++)
                byteArray[i] = (byte)msgString[i];
            return byteArray;
        }
    }
}