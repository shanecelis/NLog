using System;
using NLog;
using System.Net;
using System.Threading;

namespace Readme {
    class MainClass {
        public static void Main(string[] args) {

            clientSocketTest();

            _log.Trace("trace");
            _log.Debug("debug");
            _log.Info("info");
            _log.Warn("warning");
            _log.Error("error");
            _log.Fatal("fatal");

            Console.Read();
        }

        static readonly Logger _log = LoggerFactory.GetLogger("MyClass");

        static void clientSocketTest() {
            var defaultFormatter = new DefaultLogMessageFormatter();
            var socket = new SOSMaxAppender();
            LoggerFactory.AddAppender(((logger, logLevel, message) => {
                message = defaultFormatter.FormatMessage(logger, logLevel, message);
                socket.Send(logLevel, message);
            }));

            socket.Connect(IPAddress.Loopback, 4444);

            Thread.Sleep(50);
        }
    }
}
