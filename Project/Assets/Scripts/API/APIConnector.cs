#define GEMINI

using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

[System.Serializable]
public class PromptCharacter
{
    public string characterName;
    [SerializeField, TextArea(1, 20)] public string prompt;

    public override string ToString()
    {
        return string.Format("あなたは{0}です。{1}", characterName, prompt);
    }
}

[System.Serializable]
public class GeminiResponse
{
    public string answer;
}

[System.Serializable]
public class AnswerContent
{
    public string description;
    public string word;
}

public class APIConnector : MonoBehaviour
{
    [SerializeField] private string deployId;

    public async UniTask<string> SendRequest(string prompt, string question)
    {
#if GEMINI
        string url = $"https://script.google.com/macros/s/{deployId}/exec?question=" + UnityWebRequest.EscapeURL(question) + "&prompt=" + UnityWebRequest.EscapeURL(prompt);
        UnityWebRequest request = UnityWebRequest.Get(url);
        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"エラー:{request.error}");
            return request.error;

        }
        else
        {
            Debug.Log($"レスポンス:{request.downloadHandler.text}");
            return request.downloadHandler.text;
        }
#endif
    }
}