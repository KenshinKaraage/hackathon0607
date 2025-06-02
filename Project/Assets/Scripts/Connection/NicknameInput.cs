using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NicknameInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField nicknameInput;

    private void Start()
    {
        // ���͂��I�������Ƃ��Ƀj�b�N�l�[����ݒ肷��
        nicknameInput.onEndEdit.AddListener(SetNickname);
    }

    private void SetNickname(string input)
    {
        string inputName = nicknameInput.text.Trim();

        if (!string.IsNullOrEmpty(inputName))
        {
            PhotonNetwork.NickName = inputName;
            Debug.Log("�j�b�N�l�[���ݒ芮��: " + PhotonNetwork.NickName);
            // ���̉�ʂɐi�ނȂ炱���� Scene �J�ڂ��Ă��悢
        }
        else
        {
            Debug.LogWarning("�j�b�N�l�[������ł��I");
        }
    }
}
