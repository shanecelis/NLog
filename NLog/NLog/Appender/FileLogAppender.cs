using System.IO;

namespace NLog {
    public class FileLogAppender {
        object _lock = new object();
        string _filePath;
        bool _useColorCodes;

        public FileLogAppender(string filePath, bool useColorCodes = false) {
            _filePath = filePath;
            _useColorCodes = useColorCodes;
        }

        public void WriteLine(string message, LogLevel logLevel = LogLevel.Debug) {
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

