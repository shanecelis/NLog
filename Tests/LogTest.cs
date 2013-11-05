using NUnit.Framework;
using NLog;
using System;

[TestFixture]
public class LogTest {
    [Test]
    public void Logs() {
        Logger.RemoveAllAppender();
        Logger.AddAppender((message, logLevel) => Console.WriteLine(message));
        Log.Debug("Debug");
        Log.Info("Info");
        Log.Warn("Warn");
        Log.Error("Error");
        Log.Fatal("Fatal");
    }
}

