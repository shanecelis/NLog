using NUnit.Framework;
using Logging;

[TestFixture]
public class LoggerTest {

[Test]
    public void ManualTest() {

        var fileLogAppender = new FileLogAppender();
        fileLogAppender.filePath = "Log.txt";
        fileLogAppender.useColorCodes = true;
        Logger.AddAppender(fileLogAppender.WriteToFile);

        //Logger.Ignore(typeof(Test));
        //Logger.Ignore("Ignore");

        var logger = Logger.GetLogger(typeof(LoggerTest));
        logger.Debug("Debug");
        logger.Info("Info");
        logger.Warn("Warn");
        logger.Error("Error");
        logger.Fatal("Fatal");

        logger = Logger.GetLogger("Ignore");
        logger.Debug("Debug");
        logger.Info("Info");
        logger.Warn("Warn");
        logger.Error("Error");
        logger.Fatal("Fatal");
    }
}

