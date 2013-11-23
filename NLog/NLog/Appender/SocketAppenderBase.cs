using System;
using System.Collections.Generic;
using Gabble;

namespace NLog {
    public abstract class SocketAppenderBase {
        public TcpSocket socket { get; private set; }

        readonly List<HistoryItem> _history = new List<HistoryItem>();

        public void Connect(string ip = "127.0.0.1", int port = 4444) {
            socket = new TcpSocket();
            socket.OnConnect += onConnected;
            socket.Connect(ip, port);
        }

        public void Listen(int port, int backlog = 100) {
            socket = new TcpSocket();
            socket.OnConnect += onConnected;
            socket.Listen(port, backlog);
        }

        public void Disconnect() {
            socket.Disconnect();
        }

        public void Send(string message, LogLevel logLevel) {
            if (socket != null && socket.connected)
                socket.Send(SerializeMessage(message, logLevel));
            else
                _history.Add(new HistoryItem(message, logLevel));
        }

        void onConnected(object sender, EventArgs e) {
            if (_history.Count > 0) {
                Send("SocketAppenderBase: Flush history - - - - - - - - - - - - - - - - - - - -", LogLevel.Debug);
                foreach (HistoryItem item in _history)
                    Send(item.message, item.logLevel);

                Send("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -", LogLevel.Debug);
                _history.Clear();
            }
        }

        protected abstract byte[] SerializeMessage(string message, LogLevel logLevel);

        struct HistoryItem {
            public string message;
            public LogLevel logLevel;

            public HistoryItem(string message, LogLevel logLevel) {
                this.message = message;
                this.logLevel = logLevel;
            }
        }
    }
}