using NUnit.Framework;
using NLog;
using System.Threading;

[TestFixture]
public class FileLogAppenderTest {
    [SetUp]
    public void BeforeEach() {
        LoggerFactory.RemoveAllAppender();
        var fileLogAppender = new FileLogAppender("Log.txt", true);
        //fileLogAppender.ClearFile();
        LoggerFactory.AddAppender(fileLogAppender.WriteLine);
    }

    [Test]
    public void FileLogAppender() {
        TestHelper.LogAllLogLevels(LoggerFactory.GetLogger("FileLogAppender"));
    }

    [Test]
    public void ConcurrentTest() {
        var thread1 = new Thread(new ThreadStart(logThread1));
        var thread2 = new Thread(new ThreadStart(logThread2));
        thread1.Start();
        thread2.Start();

        Thread.Sleep(500);
        Log.Debug("Done");
    }

    void logThread1() {
        var logger = LoggerFactory.GetLogger("Logger1");
        for (int i = 0; i < 10; i++) {
            logger.Debug("Message " + i);
        }
    }

    void logThread2() {
        var logger = LoggerFactory.GetLogger("Logger2");
        for (int i = 0; i < 10; i++) {
            logger.Info("Message " + i);
        }
    }
}

