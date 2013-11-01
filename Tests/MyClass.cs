using System;
using NLog;

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
