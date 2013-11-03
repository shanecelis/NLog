using NUnit.Framework;
using System;
using NLog;

[TestFixture]
public class LoggerTest {

    [SetUp]
    public void BeforeEach() {
        Logger.RemoveAllAppender();
    }

    private void fail() {
        Assert.IsTrue(false);
    }

    [Test]
    public void WorksWithoutAppender() {
        TestHelper.LogAllLogLevels(Logger.GetLogger("NoAppender"));
    }

    [Test]
    public void AddsAppender() {
        var message = "42";
        var logMessage = "";
        Logger.AddAppender((msg, logLevel) => logMessage = msg);
        Logger.GetLogger("AddsAppender").Debug(message);
        Assert.AreNotEqual("", logMessage);
    }

    [Test]
    public void AddsMultipleAppender() {
        var message = "42";
        var logMessage1 = "";
        var logMessage2 = "";
        Logger.AddAppender((msg, logLevel) => logMessage1 = msg);
        Logger.AddAppender((msg, logLevel) => logMessage2 = msg);
        Logger.GetLogger("AddsMultipleAppender").Debug(message);
        Assert.AreNotEqual("", logMessage1);
        Assert.AreNotEqual("", logMessage2);
    }

    [Test]
    public void RemovesAppender() {
        Logger.Appender appender = (msg, logLevel) => fail();
        Logger.AddAppender(appender);
        Logger.RemoveAppender(appender);
        Logger.GetLogger("RemovesAppender").Debug("42");
    }

    [Test]
    public void RemovesMultipleAppender() {
        Logger.Appender appender1 = (msg, logLevel) => fail();
        Logger.Appender appender2 = (msg, logLevel) => fail();
        Logger.AddAppender(appender1);
        Logger.AddAppender(appender2);
        Logger.RemoveAppender(appender1);
        Logger.RemoveAppender(appender2);
        Logger.GetLogger("RemovesMultipleAppender").Debug("42");
    }

    [Test]
    public void RemovesAllAppender() {
        Logger.Appender appender1 = (msg, logLevel) => fail();
        Logger.Appender appender2 = (msg, logLevel) => fail();
        Logger.AddAppender(appender1);
        Logger.AddAppender(appender2);
        Logger.RemoveAllAppender();
        Logger.GetLogger("RemovesAllAppender").Debug("42");
    }

    [Test]
    public void IgnoresLoggerOfType() {
        Logger.AddAppender((message, logLevel) => fail());
        var type = typeof(LoggerTest);
        var logger = Logger.GetLogger(type);
        Logger.Ignore(type);
        TestHelper.LogAllLogLevels(logger);
    }
    
    [Test]
    public void IgnoresLoggerWithName() {
        Logger.AddAppender((message, logLevel) => fail());
        var name = "MyLogger";
        var logger = Logger.GetLogger(name);
        Logger.Ignore(name);
        TestHelper.LogAllLogLevels(logger);
    }
}
