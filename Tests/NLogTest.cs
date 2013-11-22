using NUnit.Framework;
using System;
using NLog;

[TestFixture]
public class NLogTest {
    [SetUp]
    public void BeforeEach() {
        LoggerFactory.RemoveAllAppender();
    }

    void fail() {
        Assert.IsTrue(false);
    }

    [Test]
    public void WorksWithoutAppender() {
        TestHelper.LogAllLogLevels(LoggerFactory.GetLogger("NoAppender"));
    }

    [Test]
    public void AddsAppender() {
        var message = "42";
        var logMessage = "";
        LoggerFactory.AddAppender((msg, logLevel) => logMessage = msg);
        LoggerFactory.GetLogger("AddsAppender").Debug(message);
        Assert.AreNotEqual("", logMessage);
    }

    [Test]
    public void AddsMultipleAppender() {
        var message = "42";
        var logMessage1 = "";
        var logMessage2 = "";
        LoggerFactory.AddAppender((msg, logLevel) => logMessage1 = msg);
        LoggerFactory.AddAppender((msg, logLevel) => logMessage2 = msg);
        LoggerFactory.GetLogger("AddsMultipleAppender").Debug(message);
        Assert.AreNotEqual("", logMessage1);
        Assert.AreNotEqual("", logMessage2);
    }

    [Test]
    public void RemovesAppender() {
        Logger.Appender appender = (msg, logLevel) => fail();
                    LoggerFactory.AddAppender(appender);
        LoggerFactory.RemoveAppender(appender);
        LoggerFactory.GetLogger("RemovesAppender").Debug("42");
    }

    [Test]
    public void RemovesMultipleAppender() {
        Logger.Appender appender1 = (msg, logLevel) => fail();
        Logger.Appender appender2 = (msg, logLevel) => fail();
        LoggerFactory.AddAppender(appender1);
        LoggerFactory.AddAppender(appender2);
        LoggerFactory.RemoveAppender(appender1);
        LoggerFactory.RemoveAppender(appender2);
        LoggerFactory.GetLogger("RemovesMultipleAppender").Debug("42");
    }

    [Test]
    public void RemovesAllAppender() {
        Logger.Appender appender1 = (msg, logLevel) => fail();
        Logger.Appender appender2 = (msg, logLevel) => fail();
        LoggerFactory.AddAppender(appender1);
        LoggerFactory.AddAppender(appender2);
        LoggerFactory.RemoveAllAppender();
        LoggerFactory.GetLogger("RemovesAllAppender").Debug("42");
    }

    [Test]
    public void SetsGlobalMinLogLevel() {
        var message = "42";
        var logMessage = "";
        LoggerFactory.globalMinLogLevel = LogLevel.Info;
        LoggerFactory.AddAppender((msg, logLevel) => logMessage = msg);
        LoggerFactory.GetLogger("AddsAppender").Debug(message);
        Assert.AreEqual("", logMessage);
    }
}
