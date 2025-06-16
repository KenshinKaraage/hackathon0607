using UnityEngine;
using TMPro;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine.UI;
using System;

public class AnswerElement : MonoBehaviour
{
    [SerializeField] private CharacterIcon characterIcon;
    [SerializeField] private TMP_Text answerText;
    [SerializeField] private Button selectButton;


    public void SetIcon(Sprite sprite, string name)
    {
        characterIcon.Set(sprite, name);
    }

    public void SetAnswerText(string answer)
    {
        answerText.text = answer;
    }

    public void HideButton()
    {
        selectButton.gameObject.SetActive(false);
    }

    public void ShowButton(IPlayerCharacter player, Action<IPlayerCharacter> onSelected)
    {
        selectButton.gameObject.SetActive(true);
        selectButton.onClick.AddListener(() => { onSelected(player); });
    }
}
