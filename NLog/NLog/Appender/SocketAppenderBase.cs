using System;
using System.Collections.Generic;
using Communico;

namespace NLog {
    public abstract class SocketAppenderBase {
        TcpSocket _socket;
        List<HistoryItem> _history = new List<HistoryItem>();

        public void Connect(string ip = "127.0.0.1", int port = 4444) {
            _socket = new TcpSocket();
            _socket.OnConnect += onConnected;
            _socket.NewConnection(ip, port);
        }

        public void Disconnect() {
            _socket.Disconnect();
        }

        public void Send(string message, LogLevel logLevel = LogLevel.Debug) {
            if (_socket.connected)
                _socket.Send(SerializeMessage(message, logLevel));
            else
                _history.Add(new HistoryItem(message, logLevel));
        }

        void onConnected(object sender, EventArgs e) {
            foreach (HistoryItem item in _history)
                Send(item.message, item.logLevel);

            _history.Clear();
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