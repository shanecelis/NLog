using NUnit.Framework;
using NLog;
using System;

// cat - | nc -l -p 4444

[TestFixture]
public class SocketAppenderTest {

//    [Test]
//    public void Sends() {
//        var s = new SocketAppender(true);
//        s.Connect();
//        s.Send("Hello");
//        s.Send("World");
//    }

    [Test]
    public void Test() {
        Logger.RemoveAllAppender();
        var s = new SocketAppender(true);
        s.Connect();
        Logger.AddAppender(s.Send);
        Logger.AddAppender((message, logLevel) => Console.WriteLine(message));
        TestHelper.LogAllLogLevels(Logger.GetLogger("SocketAppender"));
    }
}


