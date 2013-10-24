# Logging for C-Sharp

## Usage

### Setup Logging
```
Logger.AddAppender((logLevel, message) => Console.WriteLine);

var fileLogAppender = new FileLogAppender("Log.txt");
fileLogAppender.useColorCodes = true;
Logger.AddAppender(fileLogAppender.WriteToFile);

```

### Usage
```
using System;
using Logging;

public class MyClass {

    private static Logger _log = Logger.GetLogger(typeof(MyClass));

    public MyClass() {
        _log.Debug("My class");

        var success = doSth("aKey");

        if (success) {
            _log.Info("Success");
        } else {
            _log.Warn("No success");
        }
    }

    private bool doSth(string key) {
        if (key != "aKey") {
            _log.Error("Invalid key");
            return false;
        }

        _log.Debug("Doing something...");
        return true;
    }
}

```
## Output

Use tail -f to see the logfile in the terminal

```
$ tail -f Log.txt
```

![Logger output screenshot](http://sschmid.com/Dev/csharp/Libs/Logging-C-Sharp/Logging-C-Sharp_LogScreenshot1.png)

![Logger output screenshot](http://sschmid.com/Dev/csharp/Libs/Logging-C-Sharp/Logging-C-Sharp_LogScreenshot2.png)
