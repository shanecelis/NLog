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
        private static readonly string _reset   = "0m";
        private static readonly string _esc     = "\x1B[";
        // 0: background_color, 1: forground_color, 2: message
        private static readonly string _format  = _esc + "{0}" + _esc + "{1}" + "{2}" + _esc + _reset;
        // Foreground colors
        private static readonly string _fg_black   = "30m";
        private static readonly string _fg_red     = "31m";
        private static readonly string _fg_green   = "32m";
        private static readonly string _fg_yellow  = "33m";
        private static readonly string _fg_blue    = "34m";
        private static readonly string _fg_magenta = "35m";
        private static readonly string _fg_cyan    = "36m";
        private static readonly string _fg_white   = "37m";
        // Background colors
        private static readonly string _bg_none    = "";
        private static readonly string _bg_black   = "40m";
        private static readonly string _bg_red     = "41m";
        private static readonly string _bg_green   = "42m";
        private static readonly string _bg_yellow  = "43m";
        private static readonly string _bg_blue    = "44m";
        private static readonly string _bg_magenta = "45m";
        private static readonly string _bg_cyan    = "46m";
        private static readonly string _bg_white   = "47m";


        private static readonly Dictionary<LogLevel, string> _foregroundColors = new Dictionary<LogLevel, string>() {
            { LogLevel.Trace, _fg_white },
            { LogLevel.Debug, _fg_blue },
            { LogLevel.Info, _fg_green },
            { LogLevel.Warn, _fg_yellow },
            { LogLevel.Error, _fg_white },
            { LogLevel.Fatal, _fg_white }
        };

        private static readonly Dictionary<LogLevel, string> _backgroundColors = new Dictionary<LogLevel, string>() {
            { LogLevel.Trace, _bg_cyan },
            { LogLevel.Debug, _bg_none },
            { LogLevel.Info, _bg_none },
            { LogLevel.Warn, _bg_none },
            { LogLevel.Error, _bg_red },
            { LogLevel.Fatal, _bg_magenta }
        };

        public static string FormatMessage(string message, LogLevel logLevel) {
            return String.Format(_format, _backgroundColors[logLevel], _foregroundColors[logLevel], message);
        }
    }
}

