using System;
using NLog;

namespace Example {
    public class Program {

        public static void Main(string[] args) {
            LoggerFactory.globalMinLogLevel = LogLevel.On;
            LoggerFactory.AddAppender((message, logLevel) => Console.WriteLine(ColorCodeFormatter.FormatMessage(message, logLevel)));

            // Listen (requires netcat)
            // $ nc -lvvp 1234
            var sender = new SocketAppender(true);
            LoggerFactory.AddAppender(sender.Send);

            // Connect
            // $ telnet 127.0.0.1 1235
            var listener = new SocketAppender(true);
            LoggerFactory.AddAppender(listener.Send);

            sender.Connect("127.0.0.1", 1234);
            listener.Listen(1235);

            var l = LoggerFactory.GetLogger("TestLogger", LogLevel.Info);
            l.Debug("You should not see me...");
            l.Info("But me!");

            Log.Trace("Hello world!");

            Console.Read();
            Log.Warn("Bye bye");
            Console.Read();
        }
    }
}
