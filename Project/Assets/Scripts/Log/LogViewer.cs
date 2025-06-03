using TMPro;
using UnityEngine;

public class LogViewer : MonoBehaviour
{
    public TextMeshProUGUI logText;
    private string logCache = "";

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
        logCache += logString + "\n";
        if (logCache.Length > 5000)  // ’·‚·‚¬‚éê‡‚ÍØ‚é
        {
            logCache = logCache.Substring(logCache.Length - 4000);
        }

        if (logText != null)
        {
            logText.text = logCache;
        }
    }
}