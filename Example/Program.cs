using System;
using NLog;
using System.Net;
using System.Threading;

namespace Example {
    public class Program {
        public static void Main(string[] args) {
            LoggerFactory.globalLogLevel = LogLevel.On;
            consoleLogTest();
//            fileWriterTest();
//            clientSocketTest();
//            serverSocketTest();

            Log.Trace("trace");
            Log.Debug("debug");
            Log.Info("info");
            Log.Warn("warn");
            Log.Error("error");
            Log.Fatal("fatal");

            Console.Read();
        }

        static void consoleLogTest() {
            var defaultFormatter = new DefaultLogMessageFormatter();
            var timestampFormatter = new TimestampFormatter();
            var colorFormatter = new ColorCodeFormatter();
            LoggerFactory.AddAppender((logger, logLevel, message) => {
                message = defaultFormatter.FormatMessage(logger, logLevel, message);
                message = timestampFormatter.FormatMessage(logger, logLevel, message);
                message = colorFormatter.FormatMessage(logLevel, message);
                Console.WriteLine(message);
            });
        }

        static void fileWriterTest() {
            var fileWriter = new FileWriter("Log.txt");
            fileWriter.ClearFile();
            var formatter = new DefaultLogMessageFormatter();
            LoggerFactory.AddAppender((logger, logLevel, message) => {
                var logMessage = formatter.FormatMessage(logger, logLevel, message);
                fileWriter.WriteLine(logMessage);
            });
        }

        static void clientSocketTest() {
            var formatter = new DefaultLogMessageFormatter();
            var colorFormatter = new ColorCodeFormatter();
            var socket = new SocketAppender();
            LoggerFactory.AddAppender(((logger, logLevel, message) => {
                var logMessage = formatter.FormatMessage(logger, logLevel, message);
                var coloredLogMessage = colorFormatter.FormatMessage(logLevel, logMessage);
                socket.Send(logLevel, coloredLogMessage);
            }));

            Log.Trace("History");
            socket.Connect(IPAddress.Loopback, 1234);
            Thread.Sleep(50);
        }

        static void serverSocketTest() {
            var formatter = new DefaultLogMessageFormatter();
            var colorFormatter = new ColorCodeFormatter();
            var socket = new SocketAppender();
            LoggerFactory.AddAppender(((logger, logLevel, message) => {
                var logMessage = formatter.FormatMessage(logger, logLevel, message);
                var coloredLogMessage = colorFormatter.FormatMessage(logLevel, logMessage);
                socket.Send(logLevel, coloredLogMessage);
            }));

            socket.Listen(1234);
        }
    }
}
