using NUnit.Framework;
using NLog;
using System.Threading;

[TestFixture]
public class FileLogAppenderTest {

    [SetUp]
    public void BeforeEach() {
        Logger.RemoveAllAppender();
        var fileLogAppender = new FileLogAppender("Log.txt", true);
        //fileLogAppender.ClearFile();
        Logger.AddAppender(fileLogAppender.WriteLine);
    }

    [Test]
    public void AllColors() {
        var fileLogAppender = new FileLogAppender("Log.txt", false);
        fileLogAppender.ClearFile();
        fileLogAppender.WriteLine(ColorCodeFormatter.AllFormats("All colors"));
    }

    [Test]
    public void FileLogAppender() {
        TestHelper.LogAllLogLevels(Logger.GetLogger("FileLogAppender"));
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

    private void logThread1() {
        var logger = Logger.GetLogger("Logger1");
        for (int i = 0; i < 10; i++) {
            logger.Debug("Message " + i);
        }
    }

    private void logThread2() {
        var logger = Logger.GetLogger("Logger2");
        for (int i = 0; i < 10; i++) {
            logger.Info("Message " + i);
        }
    }
}

