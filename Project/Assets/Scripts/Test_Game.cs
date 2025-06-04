using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Test
{
    public class Test_Game : MonoBehaviour
    {
        [SerializeField] private PromptCharacter[] character;
        [SerializeField, TextArea(1, 20)] private string basePrompt;

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

            APIConnector apiConnector = FindAnyObjectByType<APIConnector>();
            for (int i = 0; i < tasks.Length; i++)
            {
                string prompt = character[i] + "\n" + basePrompt;
                tasks[i] = apiConnector.SendRequest(prompt, inputField.text);
            }

            string[] results = await UniTask.WhenAll(tasks);

            for (int i = 0; i < outputs.Length; i++)
            {
                GeminiResponse response = JsonUtility.FromJson<GeminiResponse>(results[i]);
                AnswerContent answer = JsonUtility.FromJson<AnswerContent>(response.answer);

                outputs[i].SetAnswer(answer.word, answer.description);
            }
        }

    }

}
