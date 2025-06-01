#define GEMINI

using UnityEngine;
using TMPro;
using UnityEngine.Networking;

namespace Test
{

    public class Test_APIConnector : MonoBehaviour
    {
        public string deployId;

        [SerializeField] private TMP_Text outputText;
        [SerializeField] private TMP_InputField inputField;

        public async void Submit()
        {
            if (string.IsNullOrEmpty(inputField.text)) return;

#if GEMINI
            //gemini
            string url = $"https://script.google.com/macros/s/{deployId}/exec?question=" + UnityWebRequest.EscapeURL(inputField.text);

#endif
            UnityWebRequest request = UnityWebRequest.Get(url);
            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("エラー: " + request.error);
            }
            else
            {
                outputText.text = request.downloadHandler.text;
                Debug.Log("レスポンス: " + request.downloadHandler.text);
            }
        }
    }
}
