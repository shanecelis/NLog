using NUnit.Framework;
using NLog;
using System;

[TestFixture]
public class LogTest {
    [Test]
    public void Logs() {
        LoggerFactory.RemoveAllAppender();
        LoggerFactory.AddAppender((message, logLevel) => Console.WriteLine(message));
        Log.Debug("Debug");
        Log.Info("Info");
        Log.Warn("Warn");
        Log.Error("Error");
        Log.Fatal("Fatal");
    }

    [Test]
    public void AssertDoesNotThrow() {
        var message = "Did not work";
        Log.Assert(true, message);
    }

    [Test]
    [ExpectedException(typeof(NLogAssert), "Did not work")]
    public void AssertThrows() {
        var message = "Did not work";
        Log.Assert(false, message);
    }
}

