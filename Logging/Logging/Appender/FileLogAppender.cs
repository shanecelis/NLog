// <summary>
// ANSI COLOR escape codes for colors and other things. 
// You can change the color of foreground and background plus bold, italic, underline etc
// 
// For a complete list see http://en.wikipedia.org/wiki/ANSI_escape_code#Colors
// </summary>

using System.Collections.Generic;
using System.IO;

namespace Logging {
    public class FileLogAppender {
        public bool useColorCodes = false;

        private string _filePath;
        private static string _reset = "\x1B[0m";
        private static string _black = "\x1B[30m";
        private static string _red = "\x1B[31m";
        private static string _green = "\x1B[32m";
        private static string _orange = "\x1B[33m";
        private static string _blue = "\x1B[34m";
        private static string _magenta = "\x1B[35m";
        private static string _cyan = "\x1B[36m";
        private static string _white = "\x1B[37m";
        private static Dictionary<LogLevel, string> _colorLookup = new Dictionary<LogLevel, string>() {
            { LogLevel.Debug, _blue },
            { LogLevel.Info, _green },
            { LogLevel.Warn, _orange },
            { LogLevel.Error, _red },
            { LogLevel.Fatal, _magenta }
        };

        public FileLogAppender(string filePath) {
            _filePath = filePath;
        }

        public void WriteToFile(LogLevel logLevel, string message) {
            using (StreamWriter writer = new StreamWriter(_filePath, true)) {
                writer.WriteLine(useColorCodes ? _colorLookup[logLevel] + message + _reset : message);
            }
        }
    }
}

