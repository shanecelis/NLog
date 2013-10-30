using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

public class SOSMaxLog {
    private Socket _socket;
    private IPEndPoint _endPoint;
    private Boolean _connected = false;
    private Boolean _connecting = false;
    private List<HistoryItem> _history = new List<HistoryItem>();

    public void Connect(string host = "127.0.0.1", int port = 4444) {
        _endPoint = new IPEndPoint(IPAddress.Parse(host), port);
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _connecting = true;


        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
        args.UserToken = _socket;
        args.RemoteEndPoint = _endPoint;
        args.Completed += (s, e) => {
            _connected = (e.SocketError == SocketError.Success);
            _connecting = false;

            if (!_connected) {
                Console.WriteLine("Socket Connect Error: {0}", e.SocketError);
            } else {
                foreach (HistoryItem item in _history) {
                    Log(item.Log, item.Level);
                }

                _history.Clear();
            }
        };

        _socket.ConnectAsync(args);
    }

    public void Disconnect() {
        if (_socket != null) {
            _socket.Close();
        }
    }

    public void Log(string message, string level = "DEBUG") {
        if (!IsConnected) {
            if (_connecting) {
                _history.Add(new HistoryItem(message, level));
            }

            return;
        }

        send(serialize(formatLogMessage(message, level)));
    }

    private string formatLogMessage(string message, string level) {
        string[] lines = message.Split('\n');
        string commandType = lines.Length == 1 ? "showMessage" : "showFoldMessage";
        Boolean isMultiLine = lines.Length > 1;

        return new StringBuilder("!SOS<")
            .Append(commandType)
                .Append(" key=\"")
                .Append(level)
                .Append("\">")
                .Append(!isMultiLine ? replaceXmlSymbols(message) : logMessage(lines[0], message))
                .Append("</")
                .Append(commandType)
                .Append(">")
                .ToString();
    }

    private string logMessage(string title, string log) {
        return new StringBuilder("<title>")
            .Append(replaceXmlSymbols(title))
                .Append("</title>")
                .Append("<message>")
                .Append(replaceXmlSymbols(log.Substring(log.IndexOf('\n') + 1)))
                .Append("</message>")
                .ToString();
    }

    private string replaceXmlSymbols(string str) {
        return str.Replace("<", "&lt;").Replace(">", "&gt;");
    }

    private byte[] serialize(string message) {
        char[] msgString = message.ToCharArray();
        byte[] byteArray = new byte[message.Length + 1];

        for (int i = 0; i < msgString.Length; ++i) {
            byteArray[i] = (byte)msgString[i];
        }

        // Must terminate with a null byte!
        byteArray[message.Length] = 0;

        return byteArray;
    }

    private void send(byte[] buffer) {
        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
        //args.SocketClientAccessPolicyProtocol = SocketClientAccessPolicyProtocol.Tcp;
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
        get {
            return _socket != null && _socket.Connected;
        }
    }

    private class HistoryItem {
        private string _name;
        private string _level;

        public HistoryItem(string log, string level) {
            this.Log = log;
            this.Level = level;
        }

        public string Log {
            get { return _name; }
            set { _name = value; }
        }

        public string Level {
            get { return _level; }
            set { _level = value; }
        }
    }
}
