using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace NLog {
    public abstract class SocketAppenderBase {
        private Socket _socket;
        private IPEndPoint _endPoint;
        private Boolean _connected = false;
        private Boolean _connecting = false;
        private List<HistoryItem> _history = new List<HistoryItem>();

        public void Connect(string host = "127.0.0.1", int port = 4444) {
            _endPoint = new IPEndPoint(IPAddress.Parse(host), port);
            connect();
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
                            Send(item.message, item.logLevel);

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

        public void Send(string message, LogLevel logLevel = LogLevel.Debug) {
            if (!IsConnected) {
                if (_connecting)
                    _history.Add(new HistoryItem(message, logLevel));

                return;
            }

            send(SerializeMessage(message, logLevel));
        }

        protected abstract byte[] SerializeMessage(string message, LogLevel logLevel);

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