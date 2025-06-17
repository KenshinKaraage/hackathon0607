using TMPro;
using UnityEngine;

public class OutputTextBox : MonoBehaviour
{
    [SerializeField] private TMP_Text characterText;

    [SerializeField] private TMP_Text wordText;
    [SerializeField] private TMP_Text descriptionText;

    public void SetCharacter(string character)
    {
        characterText.text = character;
    }

    public void SetAnswer(string word, string description)
    {
        wordText.text = word;
        descriptionText.text = description;
    }
}
