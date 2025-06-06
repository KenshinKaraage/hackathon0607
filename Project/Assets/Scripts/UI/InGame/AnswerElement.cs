using UnityEngine;
using TMPro;

public class AnswerElement : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text answerText;

    public void SetView(string name, string answer)
    {
        nameText.text = name;
        answerText.text = answer;
    }
}
