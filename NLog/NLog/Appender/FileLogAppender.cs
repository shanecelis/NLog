using System.IO;

namespace NLog {
    public class FileLogAppender {
        readonly object _lock = new object();
        readonly string _filePath;
        readonly bool _useColorCodes;

        public FileLogAppender(string filePath, bool useColorCodes) {
            _filePath = filePath;
            _useColorCodes = useColorCodes;
        }

        public void WriteLine(string message, LogLevel logLevel) {
            lock (_lock) {
                using (StreamWriter writer = new StreamWriter(_filePath, true)) {
                    writer.WriteLine(_useColorCodes ? ColorCodeFormatter.FormatMessage(message, logLevel) : message);
                }
            }
        }

        public void ClearFile() {
            lock (_lock) {
                using (StreamWriter writer = new StreamWriter(_filePath, false)) {
                    writer.Write("");
                }
            }
        }
    }
}

