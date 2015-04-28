using System;
using NLog;
using System.Net;

namespace Readme {
    public class ReadmeSnippets {

        void setupNLog(MyErrorReporter myErrorReporter) {
            LoggerFactory.globalLogLevel = LogLevel.On;

            // Add appender to print messages with Console.WriteLine
            LoggerFactory.AddAppender((logger, logLevel, message) => Console.WriteLine(message));

            // Add another appender to write messages to a file
            var fileWriter = new FileWriter("Log.txt");
            LoggerFactory.AddAppender((logger, logLevel, message) => fileWriter.WriteLine(message));

            // Or simply create your own custom appender, e.g.
            // a custom error reporter, which only sends messages
            // if the log level is at least 'error'
            LoggerFactory.AddAppender((logger, logLevel, message) => {
                if (logLevel >= LogLevel.Error) {
                    myErrorReporter.Send(logLevel + " " + message);
                }
            });
        }

        void setupExamaple() {
            // Setup appender
            var defaultFormatter = new DefaultLogMessageFormatter();
            var colorFormatter = new ColorCodeFormatter();
            var socket = new SocketAppender();
            LoggerFactory.AddAppender(((logger, logLevel, message) => {
                message = defaultFormatter.FormatMessage(logger, logLevel, message);
                message = colorFormatter.FormatMessage(logLevel, message);
                socket.Send(logLevel, message);
            }));

            // Setup as client
            socket.Connect(IPAddress.Loopback, 1234);

            // Or setup as server
            // socket.Listen(1234);
        }

        public class MyClass {
            static readonly Logger _log = LoggerFactory.GetLogger(typeof(MyClass).Name);

            public MyClass() {
                _log.Trace("trace");
                _log.Debug("debug");
                _log.Info("info");
                _log.Warn("warning");
                _log.Error("error");
                _log.Fatal("fatal");
            }
        }

        void setupSOSMaxAppender() {
            var defaultFormatter = new DefaultLogMessageFormatter();
            var socket = new SOSMaxAppender();
            LoggerFactory.AddAppender(((logger, logLevel, message) => {
                message = defaultFormatter.FormatMessage(logger, logLevel, message);
                socket.Send(logLevel, message);
            }));

            socket.Connect(IPAddress.Loopback, 4444);
        }
    }

    public class MyErrorReporter {
        public void Send(string msg) {
        }
    }
}

