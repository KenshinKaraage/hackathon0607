using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterIcon : MonoBehaviour
{
    [SerializeField] private Image characterImage;
    [SerializeField] private TMP_Text characterText;

    public void Set(Sprite sprite, string str = "")
    {
        characterImage.sprite = sprite;
        if (!string.IsNullOrEmpty(str))
        {
            characterText.text = str;
        }
    }
}
