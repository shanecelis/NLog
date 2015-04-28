using UnityEngine;
using NLog;
using System.Collections;

public class Controller : MonoBehaviour {
    static readonly Logger _log = LoggerFactory.GetLogger(typeof(Controller).Name);

    void Start() {
        _log.Trace("trace");
        _log.Debug("debug");
        _log.Info("info");
        _log.Warn("warn");
        _log.Error("error");
        _log.Fatal("fatal");

        Debug.Log("Debug.Log");

        StartCoroutine("log");
    }

    IEnumerator log() {
        while (true) {
            yield return new WaitForSeconds(1f);
            _log.Debug("log");
        }
    }
}
