using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogViewerScroll : MonoBehaviour
{
    public TextMeshProUGUI logText;
    public ScrollRect scrollRect;

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

        // �������郍�O�̓J�b�g
        if (logCache.Length > 10000)
            logCache = logCache.Substring(logCache.Length - 8000);

        logText.text = logCache;

        // �����ň�ԉ��ɃX�N���[��
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
