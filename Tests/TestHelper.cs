using NLog;

public static class TestHelper {

    public static void LogAllLogLevels(Logger logger) {
        logger.Trace("Trace");
        logger.Debug("Debug");
        logger.Info("Info");
        logger.Warn("Warn");
        logger.Error("Error");
        logger.Fatal("Fatal");
    }
}

