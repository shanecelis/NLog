using System;
using System.Collections.Generic;
using System.IO;

namespace Logging {
    public class FileLogAppender {
        public string filePath = "Log.txt";
        public bool useColorCodes = false;

        private static string none = "{0}";
        private static string red = "[31m{0}[0m";
        private static string green = "[32m{0}[0m";
        private static string orange = "[33m{0}[0m";
        private static string blue = "[34m{0}[0m";
        private static string pink = "[35m{0}[0m";
        private static Dictionary<LogLevel, string> _colorLookup = new Dictionary<LogLevel, string>() {
            { LogLevel.Debug, blue },
            { LogLevel.Info, green },
            { LogLevel.Warn, orange },
            { LogLevel.Error, red },
            { LogLevel.Fatal, pink }
        };

        public void WriteToFile(LogLevel logLevel, string message) {
            using (StreamWriter writer = new StreamWriter(filePath, true)) {
                writer.WriteLine(useColorCodes ? String.Format(_colorLookup[logLevel], message) : message);
            }
        }
    }
}

