using NUnit.Framework;
using Logging;
using System;
using System.Threading;

[TestFixture]
public class LoggerTest {

    [Test]
    public void ManualTest() {

        var fileLogAppender = new FileLogAppender("Log.txt");
        fileLogAppender.ClearFile();
        fileLogAppender.useColorCodes = true;
        Logger.AddAppender(fileLogAppender.WriteToFile);

        var sosLog = new SOSMaxLog();
        sosLog.Connect();
        Logger.AddAppender(sosLog.Log);

        Logger.AddAppender(someAppender);
        Logger.RemoveAppender(someAppender);

//        Logger.Ignore(typeof(LoggerTest));
//        Logger.Ignore("Ignore");

        var logger1 = Logger.GetLogger(typeof(LoggerTest));
        logger1.Debug("Debug");
        logger1.Info("Info");
        logger1.Warn("Warn");
        logger1.Error("Error");
        logger1.Fatal("Fatal");

        var logger2 = Logger.GetLogger("Ignore");
        logger2.Debug("Debug");
        logger2.Info("Info");
        logger2.Warn("Warn");
        logger2.Error("Error");
        logger2.Fatal("Fatal");

        new MyClass();

        sosLog.Log("Hello", LogLevel.Debug);
        sosLog.Log("Hello", LogLevel.Info );
        sosLog.Log("Hello", LogLevel.Warn );
        sosLog.Log("Hello", LogLevel.Error);
        sosLog.Log("Hello", LogLevel.Fatal);
        sosLog.Log("Hello\nMultiline1\nMultiline2");

        var specialMessage = "07/48/27/288 [WARN]  NUdpKit.UdpClient: System.Threading.ThreadAbortException: Thread was being aborted\n  at <0x00000> <unknown method>\n  at (wrapper managed-to-native) System.Net.Sockets.Socket:RecvFrom_internal (intptr,byte[],int,int,System.Net.Sockets.SocketFlags,System.Net.SocketAddress&,int&)\n  at System.Net.Sockets.Socket.ReceiveFrom_nochecks_exc (System.Byte[] buf, Int32 offset, Int32 size, SocketFlags flags, System.Net.EndPoint& remote_end, Boolean throwOnError, System.Int32& error) [0x00000] in <filename unknown>:0\n  at System.Net.Sockets.Socket.ReceiveFrom_nochecks (System.Byte[] buf, Int32 offset, Int32 size, SocketFlags flags, System.Net.EndPoint& remote_end) [0x00000] in <filename unknown>:0\n  at System.Net.Sockets.Socket.ReceiveFrom (System.Byte[] buffer, System.Net.EndPoint& remoteEP) [0x00000] in <filename unknown>:0\n  at System.Net.Sockets.UdpClient.Receive (System.Net.IPEndPoint& remoteEP) [0x00000] in <filename unknown>:0\n  at NUdpKit.UdpClient.receiveData () [0x00000] in /Users/sschmid/Work/Wooga/Dev/Unity/Projects/MultiRacer/Assets/Source/Libraries/NUdpKit/UdpClient.cs:72";
        sosLog.Log(specialMessage, LogLevel.Warn);

        var thread1 = new Thread(new ThreadStart(logThread1));
        var thread2 = new Thread(new ThreadStart(logThread2));
        thread1.Start();
        thread2.Start();

        Thread.Sleep(1000);

        logger1.Info("Done");
    }

    private void someAppender(string message, LogLevel logLevel) {
        Console.WriteLine("No no no");
    }

    private void logThread1() {
        var logger = Logger.GetLogger("logger1");
        for (int i = 0; i < 10; i++) {
            logger.Debug("Message " + i);
        }
    }

    private void logThread2() {
        var logger = Logger.GetLogger("logger2");
        for (int i = 0; i < 10; i++) {
            logger.Info("Message " + i);
        }
    }
}

