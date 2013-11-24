using System;
using System.Text;
using System.Linq;

namespace NLog {
    public class SOSMaxAppender : SocketAppenderBase {
        protected override byte[] serializeMessage(string message, LogLevel logLevel) {
            var formattedMessage = formatLogMessage(message, logLevel.ToString());
            var bytes = Encoding.UTF8.GetBytes(formattedMessage).ToList();
            bytes.Add(0);
            return bytes.ToArray();
        }

        string formatLogMessage(string message, string logLevel) {
            string[] lines = message.Split('\n');
            string commandType = lines.Length == 1 ? "showMessage" : "showFoldMessage";
            bool isMultiLine = lines.Length > 1;

            return String.Format("!SOS<{0} key=\"{1}\">{2}</{0}>",
                commandType,
                logLevel,
                !isMultiLine ? replaceXmlSymbols(message) : multilineMessage(lines[0], message));
        }

        string multilineMessage(string title, string message) {
            return String.Format("<title>{0}</title><message>{1}</message>",
                replaceXmlSymbols(title),
                replaceXmlSymbols(message.Substring(message.IndexOf('\n') + 1)));
        }

        string replaceXmlSymbols(string str) {
            return str.Replace("<", "&lt;")
                        .Replace(">", "&gt;")
                        .Replace("&lt;", "<![CDATA[<]]>")
                        .Replace("&gt;", "<![CDATA[>]]>")
                        .Replace("&", "<![CDATA[&]]>");
        }
    }
}