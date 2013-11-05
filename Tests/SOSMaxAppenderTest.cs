using NUnit.Framework;
using NLog;

[TestFixture]
public class SOSMaxAppenderTest {
    private SOSMaxAppender _sosLog;

    [SetUp]
    public void BeforeEach() {
        Logger.RemoveAllAppender();
        _sosLog = new SOSMaxAppender();
        _sosLog.Connect();
        Logger.AddAppender(_sosLog.Send);
    }

    [Test]
    public void SOSMaxAppender() {
        TestHelper.LogAllLogLevels(Logger.GetLogger("SOSMaxAppender"));
    }

    [Test]
    public void WorksWIthSpecialCharacters() {
        var specialMessage = "07/48/27/288 [WARN]  NUdpKit.UdpClient: System.Threading.ThreadAbortException: Thread was being aborted\n  at <0x00000> <unknown method>\n  at (wrapper managed-to-native) System.Net.Sockets.Socket:RecvFrom_internal (intptr,byte[],int,int,System.Net.Sockets.SocketFlags,System.Net.SocketAddress&,int&)\n  at System.Net.Sockets.Socket.ReceiveFrom_nochecks_exc (System.Byte[] buf, Int32 offset, Int32 size, SocketFlags flags, System.Net.EndPoint& remote_end, Boolean throwOnError, System.Int32& error) [0x00000] in <filename unknown>:0\n  at System.Net.Sockets.Socket.ReceiveFrom_nochecks (System.Byte[] buf, Int32 offset, Int32 size, SocketFlags flags, System.Net.EndPoint& remote_end) [0x00000] in <filename unknown>:0\n  at System.Net.Sockets.Socket.ReceiveFrom (System.Byte[] buffer, System.Net.EndPoint& remoteEP) [0x00000] in <filename unknown>:0\n  at System.Net.Sockets.UdpClient.Receive (System.Net.IPEndPoint& remoteEP) [0x00000] in <filename unknown>:0\n  at NUdpKit.UdpClient.receiveData () [0x00000] in /Users/sschmid/Work/Wooga/Dev/Unity/Projects/MultiRacer/Assets/Source/Libraries/NUdpKit/UdpClient.cs:72";
        _sosLog.Send(specialMessage, LogLevel.Warn);
    }
}

