// <summary>
// ANSI COLOR escape codes for colors and other things. 
// You can change the color of foreground and background plus bold, italic, underline etc
// 
// For a complete list see http://en.wikipedia.org/wiki/ANSI_escape_code#Colors
// </summary>

using System.Collections.Generic;
using System;

namespace NLog {
    public static class ColorCodeFormatter {
        private static string _reset   = "0m";
        private static string _red     = "31m";
        private static string _green   = "32m";
        private static string _orange  = "33m";
        private static string _blue    = "34m";
        private static string _magenta = "35m";

        private static string _esc     = "\x1B[";
        private static string _format  = _esc + "{0}{1}" + _esc + _reset; // 0: color, 1: message

        private static Dictionary<LogLevel, string> _colorLookup = new Dictionary<LogLevel, string>() {
            { LogLevel.Debug, _blue },
            { LogLevel.Info, _green },
            { LogLevel.Warn, _orange },
            { LogLevel.Error, _red },
            { LogLevel.Fatal, _magenta }
        };

        public static string FormatMessage(string message, LogLevel logLevel) {
            return String.Format(_format, _colorLookup[logLevel], message);
        }
    }
}

