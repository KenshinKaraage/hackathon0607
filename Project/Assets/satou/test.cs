using UnityEngine;
using TMPro;

public class ChangeTMPTextOnEnter : MonoBehaviour
{
    public TextMeshProUGUI targetText; // TextMeshPro��UI�p�e�L�X�g

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // Enter�L�[
        {
            targetText.text = "�G���^�[�L�[��������܂����I";
        }
    }
}
