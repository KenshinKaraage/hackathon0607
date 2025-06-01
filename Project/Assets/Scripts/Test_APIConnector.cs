using UnityEngine;
using TMPro;

namespace Test
{
    public class Test_APIConnector : MonoBehaviour
    {
        [SerializeField] private TMP_Text outputText;
        [SerializeField] private TMP_InputField inputField;

        public void Submit()
        {
            if (string.IsNullOrEmpty(inputField.text)) return;


        }
    }
}
