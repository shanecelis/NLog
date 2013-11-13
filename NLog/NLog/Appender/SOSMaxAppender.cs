using System;

namespace NLog {
    public class SOSMaxAppender : SocketAppenderBase {

        protected override byte[] SerializeMessage(string message, LogLevel logLevel) {
            message = formatLogMessage(message, logLevel.ToString());
            char[] msgString = message.ToCharArray();
            byte[] byteArray = new byte[message.Length + 1];
            for (int i = 0; i < msgString.Length; i++)
                byteArray[i] = (byte)msgString[i];
            // Must terminate with a null byte!
            byteArray[message.Length] = 0;
            return byteArray;
        }

        private string formatLogMessage(string message, string logLevel) {
            string[] lines = message.Split('\n');
            string commandType = lines.Length == 1 ? "showMessage" : "showFoldMessage";
            bool isMultiLine = lines.Length > 1;

            return String.Format("!SOS<{0} key=\"{1}\">{2}</{0}>",
                                 commandType,
                                 logLevel,
                                 !isMultiLine ? replaceXmlSymbols(message) : multilineMessage(lines[0], message));
        }

        private string multilineMessage(string title, string message) {
            return String.Format("<title>{0}</title><message>{1}</message>",
                                 replaceXmlSymbols(title),
                                 replaceXmlSymbols(message.Substring(message.IndexOf('\n') + 1)));
        }

        private string replaceXmlSymbols(string str) {
            return str.Replace("<", "&lt;")
                        .Replace(">", "&gt;")
                        .Replace("&lt;", "<![CDATA[<]]>")
                        .Replace("&gt;", "<![CDATA[>]]>")
                        .Replace("&", "<![CDATA[&]]>");
        }
    }
}