using UnityEngine;
using TMPro;
public class PlayerViewElement : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;

    public void SetView(string name)
    {
        nameText.text = name;
    }
}
