using System;
using System.Collections.Generic;
using Gabble;
using System.Net;

namespace NLog {
    public abstract class SocketAppenderBase {
        public AbstractTcpSocket socket { get; private set; }

        readonly List<HistoryItem> _history = new List<HistoryItem>();

        public void Connect(IPAddress ip, int port) {
            var client = new TcpClientSocket();
            client.OnConnect += onConnected;
            client.Connect(ip, port);
            socket = client;
        }

        public void Listen(int port) {
            var server = new TcpServerSocket();
            server.OnClientConnect += onConnected;
            server.Listen(port);
            socket = server;
        }

        public void Disconnect() {
            socket.Disconnect();
        }

        public void Send(string message, LogLevel logLevel) {
            if (isSocketReady())
                socket.Send(serializeMessage(message, logLevel));
            else
                _history.Add(new HistoryItem(message, logLevel));
        }

        bool isSocketReady() {
            return socket != null &&
            (socket is TcpClientSocket && socket.isConnected) ||
            (socket is TcpServerSocket && ((TcpServerSocket)socket).connectedClients > 0);
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

        protected abstract byte[] serializeMessage(string message, LogLevel logLevel);

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