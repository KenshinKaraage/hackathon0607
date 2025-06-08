using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ExecutePlayerButtons : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Image charaImage;
    [SerializeField] private Button button;

    public void SetView(string name, Sprite sprite)
    {
        nameText.text = name;
        if (sprite != null)
        {
            charaImage.sprite = sprite;
        }
    }

    public void AddEvent(IPlayerCharacter player ,System.Action<IPlayerCharacter> unityAction){
        button.onClick.AddListener(() => unityAction(player));
    }
}
