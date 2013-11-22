using System;
using NLog;

namespace Example {
    public class Program {
        public static void Main(string[] args) {

            Logger.AddAppender(((message, logLevel) => Console.WriteLine(ColorCodeFormatter.FormatMessage(message, logLevel))));

            // Connect
            // $ telnet 127.0.0.1 1235
            var listener = new SocketAppender(true);
            Logger.AddAppender(listener.Send);
            listener.Listen(1235);

            // Listen (requires netcat)
            // $ nc -lvvp 1234
            var sender = new SocketAppender(true);
            Logger.AddAppender(sender.Send);
            sender.Connect("127.0.0.1", 1234);

            Log.Trace("Hello world!");

            Console.Read();
        }
    }
}
