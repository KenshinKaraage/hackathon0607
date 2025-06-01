using UnityEngine;
using TMPro;

public class ChangeTMPTextOnEnter : MonoBehaviour
{
    public TextMeshProUGUI targetText; // TextMeshProのUI用テキスト

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // Enterキー
        {
            targetText.text = "エンターキーが押されました！";
        }
    }
}
