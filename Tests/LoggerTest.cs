using NUnit.Framework;
using Logging;

[TestFixture]
public class LoggerTest {

[Test]
    public void ManualTest() {

        var fileLogAppender = new FileLogAppender("Log.txt");
        fileLogAppender.useColorCodes = true;
        Logger.AddAppender(fileLogAppender.WriteToFile);

        //Logger.Ignore(typeof(Test));
        //Logger.Ignore("Ignore");

        var logger1 = Logger.GetLogger(typeof(LoggerTest));
        logger1.Debug("Debug");
        logger1.Info("Info");
        logger1.Warn("Warn");
        logger1.Assert("Assert");
        logger1.Error("Error");
        logger1.Fatal("Fatal");

        var logger2 = Logger.GetLogger("Ignore");
        logger2.Debug("Debug");
        logger2.Info("Info");
        logger2.Warn("Warn");
        logger2.Assert("Assert");
        logger2.Error("Error");
        logger2.Fatal("Fatal");
    }
}

