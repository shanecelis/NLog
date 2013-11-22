using System;
using NLog;

public class MyClass {

    static readonly Logger _log = LoggerFactory.GetLogger(typeof(MyClass));

    public MyClass() {
        _log.Debug("My class");

        var success = doSth("aKey");    
    
        if (success) {
            _log.Info("Success");
        } else {
            _log.Warn("No success");
        }
    }

    bool doSth(string key) {
        if (key != "aKey") {
            _log.Error("Invalid key");
            return false;
        }

        _log.Debug("Doing something...");
        return true;
    }
}
