using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace NLog {
    public class SOSMaxAppender {
        private Socket _socket;
        private IPEndPoint _endPoint;
        private Boolean _connected = false;
        private Boolean _connecting = false;
        private List<HistoryItem> _history = new List<HistoryItem>();

        public void Connect(string host = "127.0.0.1", int port = 4444) {
            _endPoint = new IPEndPoint(IPAddress.Parse(host), port);
        }

        public void Reconnect() {
            connect();
        }

        private void connect() {
            if (!_connecting && !_connected) {
                _connecting = true;
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.UserToken = _socket;
                args.RemoteEndPoint = _endPoint;
                args.Completed += (s, e) => {
                    _connected = (e.SocketError == SocketError.Success);
                    _connecting = false;

                    if (!_connected) {
                        Console.WriteLine("Socket Connect Error: {0}", e.SocketError);
                    } else {
                        foreach (HistoryItem item in _history)
                            Log(item.message, item.logLevel);

                        _history.Clear();
                    }
                };

                _socket.ConnectAsync(args);
            }
        }

        public void Disconnect() {
            if (_socket != null)
                _socket.Close();
        }

        public void Log(string message, LogLevel logLevel = LogLevel.Debug) {
            if (!IsConnected) {
                if (_connecting)
                    _history.Add(new HistoryItem(message, logLevel));

                return;
            }

            send(serialize(formatLogMessage(message, logLevel.ToString())));
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

        private byte[] serialize(string message) {
            char[] msgString = message.ToCharArray();
            byte[] byteArray = new byte[message.Length + 1];
            for (int i = 0; i < msgString.Length; ++i)
                byteArray[i] = (byte)msgString[i];
            // Must terminate with a null byte!
            byteArray[message.Length] = 0;
            return byteArray;
        }

        private void send(byte[] buffer) {
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.SetBuffer(buffer, 0, buffer.Length);
            args.UserToken = _socket;
            args.RemoteEndPoint = _endPoint;
            args.Completed += (s, e) => {
                if (e.SocketError != SocketError.Success) {
                    _connected = false;
                    _socket.Close();
                }
            };

            _socket.SendAsync(args);
        }

        public Boolean IsConnected {
            get { return _socket != null && _socket.Connected; }
        }

        private struct HistoryItem {
            public string message;
            public LogLevel logLevel;

            public HistoryItem(string message, LogLevel logLevel) {
                this.message = message;
                this.logLevel = logLevel;
            }
        }
    }
}