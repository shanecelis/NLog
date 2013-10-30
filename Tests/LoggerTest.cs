using NUnit.Framework;
using Logging;

[TestFixture]
public class LoggerTest {

    [Test]
    public void ManualTest() {

        var fileLogAppender = new FileLogAppender("Log.txt");
        fileLogAppender.useColorCodes = true;
        Logger.AddAppender(fileLogAppender.WriteToFile);

        var sosLog = new SOSMaxLog();
        sosLog.Connect();
        Logger.AddAppender((logLevel, message) => sosLog.Log(message, logLevel.ToString()));


        Logger.Ignore(typeof(LoggerTest));
        Logger.Ignore("Ignore");

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

        sosLog.Log("Hello", SOSMaxLog.DEBUG);
        sosLog.Log("Hello", SOSMaxLog.INFO);
        sosLog.Log("Hello", SOSMaxLog.WARN);
        sosLog.Log("Hello", SOSMaxLog.ERROR);
        sosLog.Log("Hello", SOSMaxLog.FATAL);
        sosLog.Log("Hello\nMultiline1\nMultiline2");

        var specialMessage = "07/48/27/288 [WARN]  NUdpKit.UdpClient: System.Threading.ThreadAbortException: Thread was being aborted\n  at <0x00000> <unknown method>\n  at (wrapper managed-to-native) System.Net.Sockets.Socket:RecvFrom_internal (intptr,byte[],int,int,System.Net.Sockets.SocketFlags,System.Net.SocketAddress&,int&)\n  at System.Net.Sockets.Socket.ReceiveFrom_nochecks_exc (System.Byte[] buf, Int32 offset, Int32 size, SocketFlags flags, System.Net.EndPoint& remote_end, Boolean throwOnError, System.Int32& error) [0x00000] in <filename unknown>:0\n  at System.Net.Sockets.Socket.ReceiveFrom_nochecks (System.Byte[] buf, Int32 offset, Int32 size, SocketFlags flags, System.Net.EndPoint& remote_end) [0x00000] in <filename unknown>:0\n  at System.Net.Sockets.Socket.ReceiveFrom (System.Byte[] buffer, System.Net.EndPoint& remoteEP) [0x00000] in <filename unknown>:0\n  at System.Net.Sockets.UdpClient.Receive (System.Net.IPEndPoint& remoteEP) [0x00000] in <filename unknown>:0\n  at NUdpKit.UdpClient.receiveData () [0x00000] in /Users/sschmid/Work/Wooga/Dev/Unity/Projects/MultiRacer/Assets/Source/Libraries/NUdpKit/UdpClient.cs:72";
        sosLog.Log(specialMessage, SOSMaxLog.WARN);
    }
}

