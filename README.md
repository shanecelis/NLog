# NLog – Logging for C-Sharp
[![Build Status](https://travis-ci.org/sschmid/NLog.svg?branch=master)](https://travis-ci.org/sschmid/NLog)

### Setup Logging
```
LoggerFactory.globalLogLevel = LogLevel.On;

// Log messages with Console.WriteLine
LoggerFactory.appenders += (logger, logLevel, message) => {
    Console.WriteLine(message);
};

// Write log to a file
var fileWriter = new FileWriter("Log.txt");
LoggerFactory.appenders += (logger, logLevel, message) => {
    fileWriter.WriteLine(message);
};

// Or simply create your own custom appender
LoggerFactory.appenders += (logger, logLevel, message) => {
    myErrorReporter.Send(logLevel + " " + message);
};
```

### Example
```
public class MyClass {
    static readonly Logger _log = LoggerFactory.GetLogger(typeof(MyClass).Name);

    public MyClass() {
        _log.Debug("It works!");
    }
}
```
![Logger output screenshot](http://sschmid.com/Dev/csharp/Libs/NLog/NLog_LogScreenshot4.png)

### Usage
```
// Create a logger instance
var logger = LoggerFactory.GetLogger("MyLogger");
logger.Info("Hello");

// Or use singleton logger
Log.Trace("trace");
Log.Debug("debug");
Log.Info("info");
Log.Warn("warn");
Log.Error("error");
Log.Fatal("fatal");
```
![Logger output screenshot](http://sschmid.com/Dev/csharp/Libs/NLog/NLog_LogScreenshot1.png)

### Make it pretty
```
var formatter = new DefaultLogMessageFormatter();
var colorFormatter = new ColorCodeFormatter();
LoggerFactory.appenders += (logger, logLevel, message) => {
    var logMessage = formatter.FormatMessage(logger, logLevel, message);
    var coloredLogMessage = colorFormatter.FormatMessage(logLevel, logMessage);
    Console.WriteLine(coloredLogMessage);
};
```
![Logger output screenshot](http://sschmid.com/Dev/csharp/Libs/NLog/NLog_LogScreenshot2.png)

### Send messages OTA via TCP
```
var socket = new SocketAppender();
LoggerFactory.appenders += ((logger, logLevel, message) => {
    var logMessage = formatter.FormatMessage(logger, logLevel, message);
    var coloredLogMessage = colorFormatter.FormatMessage(logLevel, logMessage);
    socket.Send(logLevel, coloredLogMessage);
});

// Setup as client
socket.Connect(IPAddress.Loopback, 1234);

// Or setup as server
socket.Listen(1234);
```

### Use command line tool (or e.g. telnet/netcat) to receive messages in your terminal
![Logger output screenshot](http://sschmid.com/Dev/csharp/Libs/NLog/NLog_LogScreenshot3.png)
