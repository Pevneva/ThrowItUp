using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogPrint : MonoBehaviour
{
    private string _myLog;
    private Queue _myLogQueue = new Queue();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        _myLog = logString;
        string newString = "\n [" + type + "] : " + _myLog;
        _myLogQueue.Enqueue(newString);
        if (type == LogType.Exception)
        {
            newString = "\n" + stackTrace;
            _myLogQueue.Enqueue(newString);
        }
        _myLog = string.Empty;
        foreach (string mylog in _myLogQueue)
        {
            _myLog += mylog;
        }
    }

    void OnGUI()
    {
        GUILayout.Label(_myLog);
    }
}