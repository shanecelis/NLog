using System;
using NLog;
using System.Net;
using System.Text;

namespace NLog.CommandLineTool {
    class Program {
        static AbstractTcpSocket _socket;

        public static void Main(string[] args) {
            LoggerFactory.globalLogLevel = LogLevel.Warn;

            foreach (var arg in args) {
                if (arg == "-v") {
                    LoggerFactory.globalLogLevel = LogLevel.Info;
                }
                if (arg == "-vv") {
                    LoggerFactory.globalLogLevel = LogLevel.On;
                }
            }

            LoggerFactory.AddAppender((logger, logLevel, message) => Console.WriteLine(message));

            if (args.Length == 0) {
                printUsage();
            } else if (args[0] == "-l") {
                initServer(args);
            } else if (args[0] == "-c") {
                initClient(args);
            } else {
                printUsage();
            }
        }

        static void printUsage() {
            Console.WriteLine(@"usage:
nlog [-l port]    - listen on port 
nlog [-c ip port] - connect to ip on port
[-v]              - verbose output
[-vv]             - even more verbose output
");
        }

        static void initServer(string[] args) {
            int port = 0;
            try {
                port = int.Parse(args[1]);
                var server = new TcpServerSocket();
                server.OnDisconnect += onDisconnect;
                server.Listen(port);
                _socket = server;
                start();
            } catch (Exception) {
                Console.WriteLine("Invalid port");
            }
        }

        static void initClient(string[] args) {
            IPAddress ip = null;
            int port = 0;
            try {
                ip = IPAddress.Parse(args[1]);
                port = int.Parse(args[2]);
                var client = new TcpClientSocket();
                client.OnDisconnect += onDisconnect;
                client.Connect(ip, port);
                _socket = client;
                start();
            } catch (Exception) {
                Console.WriteLine("Please specify a valid ip and port");
            }
        }

        static void start() {
            _socket.OnReceive += onReceive;
            Console.CancelKeyPress += onCancel;
            while (true) {
                _socket.Send(Encoding.UTF8.GetBytes(Console.ReadLine() + "\n"));
            }
        }

        static void onReceive(object sender, ReceiveEventArgs e) {
            if (e.bytes.Length > 1) {
                var message = new byte[e.bytes.Length - 1];
                Array.Copy(e.bytes, message, message.Length);
                Console.WriteLine(Encoding.UTF8.GetString(message));
            }
        }

        static void onCancel(object sender, ConsoleCancelEventArgs e) {
            _socket.Disconnect();
        }

        static void onDisconnect(object sender, EventArgs e) {
            Environment.Exit(0);
        }
    }
}
