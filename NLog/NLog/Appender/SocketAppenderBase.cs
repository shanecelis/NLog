using System;
using System.Collections.Generic;
using Communico;

namespace NLog {
    public abstract class SocketAppenderBase {
        TcpSocket _socket;
        readonly List<HistoryItem> _history = new List<HistoryItem>();

        public void Connect(string ip = "127.0.0.1", int port = 4444) {
            _socket = new TcpSocket();
            _socket.OnConnect += onConnected;
            _socket.Connect(ip, port);
        }

        public void Listen(int port, int backlog = 100) {
            _socket = new TcpSocket();
            _socket.OnConnect += onConnected;
            _socket.Listen(port, backlog);
        }

        public void Disconnect() {
            _socket.Disconnect();
        }

        public void Send(string message, LogLevel logLevel) {
            if (_socket != null && _socket.connected)
                _socket.Send(SerializeMessage(message, logLevel));
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