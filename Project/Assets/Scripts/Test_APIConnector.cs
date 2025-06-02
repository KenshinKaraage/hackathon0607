#define GEMINI

using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using static UnityEditor.Rendering.CameraUI;

namespace Test
{
    [System.Serializable]
    public class PromptCharacter
    {
        public string characterName;
        [SerializeField, TextArea(1,20)]public string prompt;

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

    public class Test_APIConnector : MonoBehaviour
    {
        public string deployId;
        [SerializeField] private PromptCharacter[] character;
        [SerializeField, TextArea(1,20)] private string basePrompt;

        [SerializeField] private OutputTextBox[] outputs;
        [SerializeField] private TMP_InputField inputField;

        private void Start()
        {
            for (int i = 0; i < outputs.Length; i++)
            {
                outputs[i].SetCharacter(character[i].characterName);
            }
        }

        public async void Submit()
        {
            if (string.IsNullOrEmpty(inputField.text)) return;

            UniTask<string>[] tasks = new UniTask<string>[outputs.Length];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = SendRequest(i);
            }
            string[] results = await UniTask.WhenAll(tasks);

            for (int i = 0; i < outputs.Length; i++)
            {
                GeminiResponse response = JsonUtility.FromJson<GeminiResponse>(results[i]);
                AnswerContent answer = JsonUtility.FromJson<AnswerContent>(response.answer);

                outputs[i].SetAnswer(answer.word, answer.description);
            }
        }

        public async UniTask<string> SendRequest(int promptIndex)
        {
            if (promptIndex >= character.Length) promptIndex = 0;

#if GEMINI
            string prompt = character[promptIndex] + "\n" + basePrompt;

            string url = $"https://script.google.com/macros/s/{deployId}/exec?question=" + UnityWebRequest.EscapeURL(inputField.text) + "&prompt=" + UnityWebRequest.EscapeURL(prompt);
            UnityWebRequest request = UnityWebRequest.Get(url);
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"[{promptIndex}]エラー:{request.error}");
                return request.error;

            }
            else
            {
                Debug.Log($"[{promptIndex}]レスポンス:{request.downloadHandler.text}");
                return request.downloadHandler.text;
            }
#endif
        }
    }
}
